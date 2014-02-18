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
    public class UserController : BaseController
    {
        public ViewResult Index()
        {
            GetPermission();
            ViewData["toolbarButtons"] = BuildToolBarButtons();
            return View();
        }

        private bool permissionModuleSetup = false;
        private bool permissionPermissionSetup = false;
        private bool permissionDataSetup = false;
        public override void GetPermission()
        {
            this.permissionAdd = this.IsAuthorized("User.Add");
            this.permissionDelete = this.IsAuthorized("User.Delete");
            this.permissionEdit = this.IsAuthorized("User.Edit");
            this.permissionExport = this.IsAuthorized("User.Export");
            this.permissionModuleSetup = this.IsAuthorized("Module.Setup");
            this.permissionPermissionSetup = this.IsAuthorized("Permission.Setup");
            this.permissionDataSetup = this.IsAuthorized("Data.Setup");
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
            sb.Append(string.Format(linkbtn_template, "moduleSetup", "icon-user_moudle", permissionModuleSetup ? "" : "disabled=\"True\"", "设置菜单访问", "设置菜单访问", "moduleSetup()"));
            sb.Append(string.Format(linkbtn_template, "permissionSetup", "icon-user_permission", permissionPermissionSetup ? "" : "disabled=\"True\"", "设置操作权限", "设置操作权限", "permissionSetup()"));
            sb.Append(string.Format(linkbtn_template, "dataSetup", "icon-user_data", permissionDataSetup ? "" : "disabled=\"True\"", "设置数据权限", "设置数据权限", "dataSetup()"));
            return sb.ToString();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult SetupUserModule()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(UserType obj)
        {
            obj.LastVisit = DateTime.Now;
           
            RoleType role = this.Get<RoleType>(obj.RoleId);
            if (role != null)
                obj.RoleName = role.Realname;
            OrganizeType company = this.Get<OrganizeType>(obj.CId);
            if (company != null)
                obj.CompanyName = company.FullName;
            OrganizeType department = this.Get<OrganizeType>(obj.DId);
            if (department != null)
                obj.DepartmentName = department.FullName;

            bool isOk = Save(obj);
            return Json(new { IsSuccess = isOk });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public UserType GetById(int Id)
        {
            UserType obj = Get<UserType>(Id);
            return obj;
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            UserType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(UserType obj)
        {
            bool isOk = Update<UserType>(obj);
            return Json(new { IsSuccess = isOk });
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            bool isOk = DeleteObj<UserType>(id);
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
            IList<UserType> objList = NSession.CreateQuery("from UserType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<UserType>();

            object count = NSession.CreateQuery("select count(Id) from UserType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }
        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Login(string u, string p, int t)
        {
            bool iscon = Common.LoginByUser(u, p, NSession);
            if (iscon)
            {
                Common.CreateCookies(u, p, t);
                return Json(new { IsSuccess = true });
            }
            return Json(new { IsSuccess = false, Result = "用户名或者密码出错" });
        }

        public ActionResult LogOff()
        {
            Common.ClearCookies();
            return View("Login");

        }


    }
}

