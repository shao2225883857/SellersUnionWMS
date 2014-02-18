using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Sellers.WMS.Domain;
using Sellers.WMS.Utils.Extensions;
using NHibernate;
using Sellers.WMS.Web.Controllers.Filters;

namespace Sellers.WMS.Web.Controllers
{
    public class ModuleController : BaseController
    {
        public ViewResult Index()
        {
            GetPermission();
            ViewData["toolbarButtons"] = BuildToolBarButtons();
            return View();
        }

        public override void GetPermission()
        {
            this.permissionAdd = this.IsAuthorized("Module.Add");
            this.permissionDelete = this.IsAuthorized("Module.Delete");
            this.permissionEdit = this.IsAuthorized("Module.Edit");
            this.permissionExport = this.IsAuthorized("Module.Export");
        }

        /// <summary>  
        /// 加载工具栏  
        /// </summary>  
        /// <returns>工具栏HTML</returns>  
        public override string BuildToolBarButtons(int t = 0)
        {
            StringBuilder sb = new StringBuilder();
            string linkbtn_template = "<a id=\"a_{0}\" class=\"easyui-linkbutton\" style=\"float:left\"  plain=\"true\" href=\"javascript:;\" icon=\"{1}\"  {2} title=\"{3}\" onclick='{5}'>{4}</a>";
            sb.Append("<a id=\"a_refresh\" class=\"easyui-linkbutton\" style=\"float:left\"  plain=\"true\" href=\"javascript:;\" icon=\"icon-reload\"  title=\"重新加载\"  onclick='refreshClick()'>刷新</a> ");
            sb.Append("<div class='datagrid-btn-separator'></div> ");
            sb.Append(string.Format(linkbtn_template, "add", "icon-add", permissionAdd ? "" : "disabled=\"True\"", "添加用户", "添加", "addClick()"));
            sb.Append(string.Format(linkbtn_template, "edit", "icon-edit", permissionEdit ? "" : "disabled=\"True\"", "修改用户", "修改", "editClick()"));
            sb.Append(string.Format(linkbtn_template, "delete", "icon-remove", permissionDelete ? "" : "disabled=\"True\"", "删除用户", "删除", "delClick()"));
            sb.Append("<div class='datagrid-btn-separator'></div> ");
            sb.Append("<a href=\"#\" class='easyui-menubutton' " + (permissionExport ? "" : "disabled='True'") + " data-options=\"menu:'#dropdown',iconCls:'icon-undo'\">导出</a>");

            return sb.ToString();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(ModuleType obj)
        {
            bool isOk = Save(obj);
            return Json(new { IsSuccess = isOk });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ModuleType GetById(int Id)
        {
            ModuleType obj = Get<ModuleType>(Id);
            return obj;
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            ModuleType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(ModuleType obj)
        {
            bool isOk = Update<ModuleType>(obj);
            return Json(new { IsSuccess = isOk });
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            bool isOk = DeleteObj<ModuleType>(id);
            return Json(new { IsSuccess = isOk });
        }

        public JsonResult List(int page, int rows, string sort, string order, string search)
        {
            string where = "";
            string orderby = " order by Id desc ";
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }

            if (!string.IsNullOrEmpty(search))
            {
                where = StringUtil.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where " + where;
                }
            }
            IList<ModuleType> objList = NSession.CreateQuery("from ModuleType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<ModuleType>();

            object count = NSession.CreateQuery("select count(Id) from ModuleType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult ParentList()
        {
            IList<ModuleType> objList = NSession.CreateQuery("from ModuleType").List<ModuleType>();
            IList<ModuleType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();
            SystemTree tree = new SystemTree { id = "0", text = "根目录" };
            List<SystemTree> trees = new List<SystemTree>();
            foreach (ModuleType item in fristList)
            {
                List<ModuleType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree(fooList);
                tree.children.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName, children = tree2 });
                GetChildren(objList, item, tree2);

            }
            trees.Add(tree);
            return Json(trees);
        }

        public JsonResult ALLList()
        {
            IList<ModuleType> objList = NSession.CreateQuery("from ModuleType").List<ModuleType>();
            IList<ModuleType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();


            foreach (ModuleType item in fristList)
            {
                List<ModuleType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                GetChildren(objList, item);
            }
            return Json(new { total = fristList.Count, rows = fristList });
        }

        public JsonResult TreeByUser(int id)
        {
            return SelectList(id, ResourceCategoryEnum.User.ToString());
        }

        public JsonResult TreeByRole(int id)
        {
            return SelectList(id, ResourceCategoryEnum.Role.ToString());
        }

        public JsonResult TreeByDepartment(int id)
        {
            return SelectList(id, ResourceCategoryEnum.Department.ToString());
        }

        private JsonResult SelectList(int id, string type)
        {
            IList<ModuleType> objList = NSession.CreateQuery("from ModuleType").List<ModuleType>();
            //获得这个类型的菜单权限
            List<PermissionScopeType> scopeList = NSession.CreateQuery("from PermissionScopeType where ResourceCategory=:p1 and ResourceId=:p2 and TargetCategory =:p3").SetString("p1", type).SetInt32("p2", id).SetString("p3", TargetCategoryEnum.Module.ToString()).List<PermissionScopeType>().ToList<PermissionScopeType>();

            IList<ModuleType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();
            List<SystemTree> tree = new List<SystemTree>();

            SystemTree root = new SystemTree { id = "0", text = "所有菜单" };
            if (scopeList.FindIndex(p => p.TargetId == 0) != -1)
                root.@checked = true;
            foreach (ModuleType item in fristList)
            {
                bool ischecked = scopeList.FindIndex(p => p.TargetId == item.Id) != -1;
                List<ModuleType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree(fooList, scopeList);
                root.children.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName, children = tree2, @checked = ischecked });
                GetChildren(objList, item, tree2);
            }
            tree.Add(root);
            return Json(tree);
        }

        public List<SystemTree> ConvertToTree(List<ModuleType> fooList, List<PermissionScopeType> scopeList = null)
        {
            List<SystemTree> tree = new List<SystemTree>();
            foreach (ModuleType item in fooList)
            {
                if (scopeList == null)
                    tree.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName });
                else
                {
                    if (scopeList.FindIndex(p => p.TargetId == item.Id) >= 0)
                    {
                        tree.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName, @checked = true });
                    }
                    else
                    {
                        tree.Add(new SystemTree { id = item.Id.ToString(), text = item.FullName });
                    }
                }
            }
            return tree;
        }

        private void GetChildren(IList<ModuleType> objList, ModuleType item)
        {
            foreach (ModuleType k in item.children)
            {
                List<ModuleType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                GetChildren(objList, k);
            }
        }

        private void GetChildren(IList<ModuleType> objList, ModuleType item, List<SystemTree> trees)
        {
            foreach (ModuleType k in item.children)
            {
                SystemTree tree = trees.Find(p => p.id == k.Id.ToString());
                List<ModuleType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                List<SystemTree> mlist = ConvertToTree(kList);
                tree.children = mlist;
                GetChildren(objList, k, mlist);
            }
        }

        public JsonResult CreateModuleByUser(int m, int k)
        {
            bool isOk = false;
            PermissionScopeType permissionScope = new PermissionScopeType();
            permissionScope.ResourceCategory = ResourceCategoryEnum.User.ToString();
            permissionScope.TargetCategory = TargetCategoryEnum.Module.ToString();
            permissionScope.ResourceId = k;
            permissionScope.TargetId = m;
            Save(permissionScope);

            return Json(new { IsSuccess = isOk });
        }

        public JsonResult DeleteModuleByUser(int m, int k, string ids)
        {
            bool isOk = false;

            NSession.Delete(string.Format("from  PermissionScopeType where TargetId={0} and TargetCategory='{1}' and ResourceCategory='{2}' and ResourceId={3}", m, TargetCategoryEnum.Module.ToString(), ResourceCategoryEnum.User.ToString(), k));

            if (!string.IsNullOrEmpty(ids))
            {
                NSession.Delete(string.Format("from PermissionScopeType where TargetId in({0}) and TargetCategory='{1}' and ResourceCategory='{2}' and ResourceId={3}", ids, TargetCategoryEnum.Module.ToString(), ResourceCategoryEnum.User.ToString(), k));
            }

            NSession.Flush();
            return Json(new { IsSuccess = isOk });
        }

    }
}

