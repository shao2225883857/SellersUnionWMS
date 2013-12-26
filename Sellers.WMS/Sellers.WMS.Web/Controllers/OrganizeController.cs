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

namespace Sellers.WMS.Web.Controllers
{
    public class OrganizeController : BaseController
    {
        public ViewResult Index()
        {
            GetPermission();
            ViewData["toolbarButtons"] = BuildToolBarButtons();
            return View();
        }

        public override void GetPermission()
        {
            this.permissionAdd = this.IsAuthorized("Organize.Add");
            this.permissionDelete = this.IsAuthorized("Organize.Delete");
            this.permissionEdit = this.IsAuthorized("Organize.Edit");
            this.permissionExport = this.IsAuthorized("Organize.Export");
        }

        /// <summary>  
        /// 加载工具栏  
        /// </summary>  
        /// <returns>工具栏HTML</returns>  
        public override string BuildToolBarButtons()
        {
            StringBuilder sb = new StringBuilder();
            string linkbtn_template = "<a id=\"a_{0}\" class=\"easyui-linkbutton\" style=\"float:left\"  plain=\"true\" href=\"javascript:;\" icon=\"{1}\"  {2} title=\"{3}\" onclick='{5}'>{4}</a>";
            sb.Append("<a id=\"a_refresh\" class=\"easyui-linkbutton\" style=\"float:left\"  plain=\"true\" href=\"javascript:;\" icon=\"icon-reload\"  title=\"重新加载\"  onclick='refreshClick()'>刷新</a> ");
            sb.Append("<div class='datagrid-btn-separator'></div> ");
            sb.Append(string.Format(linkbtn_template, "add", "icon-user_add", permissionAdd ? "" : "disabled=\"True\"", "添加用户", "添加", "addClick()"));
            sb.Append(string.Format(linkbtn_template, "edit", "icon-user_edit", permissionEdit ? "" : "disabled=\"True\"", "修改用户", "修改", "editClick()"));
            sb.Append(string.Format(linkbtn_template, "delete", "icon-user_delete", permissionDelete ? "" : "disabled=\"True\"", "删除用户", "删除", "delClick()"));
            sb.Append("<div class='datagrid-btn-separator'></div> ");
            sb.Append("<a href=\"#\" class='easyui-menubutton' " + (permissionExport ? "" : "disabled='True'") + " data-options=\"menu:'#dropdown',iconCls:'icon-export'\">导出</a>");

            return sb.ToString();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(OrganizeType obj)
        {
            bool isOk = Save(obj);
            return Json(new { IsSuccess = isOk });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OrganizeType GetById(int Id)
        {
            OrganizeType obj = Get<OrganizeType>(Id);
            return obj;
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            OrganizeType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(OrganizeType obj)
        {
            bool isOk = Update<OrganizeType>(obj);
            return Json(new { IsSuccess = isOk });
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            bool isOk = DeleteObj<OrganizeType>(id);
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
            IList<OrganizeType> objList = NSession.CreateQuery("from OrganizeType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<OrganizeType>();

            object count = NSession.CreateQuery("select count(Id) from OrganizeType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        public JsonResult ParentList()
        {
            IList<OrganizeType> objList = NSession.CreateQuery("from OrganizeType").List<OrganizeType>();
            IList<OrganizeType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();
            SystemTree tree = new SystemTree { id = "0", text = "根目录" };
            List<SystemTree> trees = new List<SystemTree>();
            foreach (OrganizeType item in fristList)
            {
                List<OrganizeType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
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
            IList<OrganizeType> objList = NSession.CreateQuery("from OrganizeType").List<OrganizeType>();
            IList<OrganizeType> fristList = objList.Where(p => p.ParentId == 0).OrderByDescending(p => p.SortCode).ToList();


            foreach (OrganizeType item in fristList)
            {
                List<OrganizeType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                GetChildren(objList, item);
            }
            return Json(new { total = fristList.Count, rows = fristList });
        }


        public JsonResult ALLTreeList()
        {
            IList<OrganizeType> objList = NSession.CreateQuery("from OrganizeType ORDER BY ParentId asc,SortCode desc ").List<OrganizeType>();

            List<SystemTree> tree = new List<SystemTree>();

            foreach (OrganizeType item in objList.Where(p => p.ParentId == 0))
            {

                List<OrganizeType> fooList =
                    objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree(fooList);
                tree.Add(new SystemTree { id = item.Id.ToString(), text = item.ShortName, children = tree2 });
                GetChildren(objList, item, tree2);

            }
            return Json(tree);
        }

        public JsonResult DepartmentList(int id)
        {


            List<SystemTree> tree = new List<SystemTree>();
            string sql = @"DECLARE @ID int 
                        SET @ID = 1;
                        WITH T AS 
                        (
                          SELECT * 
                          FROM Organizes 
                          WHERE ID = @ID  --查询根节点
                          UNION ALL
                          SELECT A.* 
                          FROM Organizes AS A JOIN T AS B ON A.ParentId = B.Id  --递归查询下级节点
                        )
                        SELECT * FROM T where ID<>@ID ORDER BY ParentId asc,SortCode desc";
            IList<OrganizeType> objList = NSession.CreateSQLQuery(sql).AddEntity("T", typeof(OrganizeType)).List<OrganizeType>();

            foreach (OrganizeType item in objList.Where(p => p.ParentId == id))
            {
                List<OrganizeType> fooList = objList.Where(p => p.ParentId == item.Id).OrderByDescending(p => p.SortCode).ToList();
                item.children = fooList;
                List<SystemTree> tree2 = ConvertToTree(fooList);
                tree.Add(new SystemTree { id = item.Id.ToString(), text = item.ShortName, children = tree2 });
                GetChildren(objList, item, tree2);


            }
            return Json(tree);
        }

        private void GetChildren(IList<OrganizeType> objList, OrganizeType item)
        {
            foreach (OrganizeType k in item.children)
            {
                List<OrganizeType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                GetChildren(objList, k);
            }
        }

        private void GetChildren(IList<OrganizeType> objList, OrganizeType item, List<SystemTree> trees)
        {
            foreach (OrganizeType k in item.children)
            {
                SystemTree tree = trees.Find(p => p.id == k.Id.ToString());
                List<OrganizeType> kList = objList.Where(p => p.ParentId == k.Id).OrderByDescending(p => p.SortCode).ToList();
                k.children = kList;
                List<SystemTree> mlist = ConvertToTree(kList);
                tree.children = mlist;
                GetChildren(objList, k, mlist);
            }
        }

        public List<SystemTree> ConvertToTree(List<OrganizeType> fooList, List<PermissionScopeType> scopeList = null)
        {
            List<SystemTree> tree = new List<SystemTree>();
            foreach (OrganizeType item in fooList)
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

    }
}

