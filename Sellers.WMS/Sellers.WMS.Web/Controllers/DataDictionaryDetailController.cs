﻿using System;
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
    public class DataDictionaryDetailController : BaseController
    {
        public ViewResult Index()
        {
            GetPermission();
            ViewData["toolbarButtons"] = BuildToolBarButtons();
            return View();
        }

        public override void GetPermission()
        {
            this.permissionAdd = this.IsAuthorized("DataDictionaryDetail.Add");
            this.permissionDelete = this.IsAuthorized("DataDictionaryDetail.Delete");
            this.permissionEdit = this.IsAuthorized("DataDictionaryDetail.Edit");
            this.permissionExport = this.IsAuthorized("DataDictionaryDetail.Export");
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
            sb.Append(string.Format(linkbtn_template, "add", "icon-user_add", permissionAdd ? "" : "disabled=\"True\"", "添加用户", "添加","addClick()"));
            sb.Append(string.Format(linkbtn_template, "edit", "icon-user_edit", permissionEdit ? "" : "disabled=\"True\"", "修改用户", "修改", "editClick()"));
            sb.Append(string.Format(linkbtn_template, "delete", "icon-user_delete", permissionDelete ? "" : "disabled=\"True\"", "删除用户", "删除", "delClick()"));
            sb.Append("<div class='datagrid-btn-separator'></div> ");
            sb.Append("<a href=\"#\" class='easyui-menubutton' " + (permissionExport? "": "disabled='True'") + " data-options=\"menu:'#dropdown',iconCls:'icon-export'\">导出</a>");

            return sb.ToString();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(DataDictionaryDetailType obj)
        {
            bool isOk = Save(obj);
            return Json(new { IsSuccess = isOk });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public  DataDictionaryDetailType GetById(int Id)
        {
            DataDictionaryDetailType obj = Get<DataDictionaryDetailType>(Id);
            return obj;
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            DataDictionaryDetailType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(DataDictionaryDetailType obj)
        {
            bool isOk = Update<DataDictionaryDetailType>(obj);
            return Json(new { IsSuccess = isOk });
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
			bool isOk = Delete<DataDictionaryDetailType>(id);
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
            IList<DataDictionaryDetailType> objList = NSession.CreateQuery("from DataDictionaryDetailType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<DataDictionaryDetailType>();

            object count = NSession.CreateQuery("select count(Id) from DataDictionaryDetailType " + where ).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

