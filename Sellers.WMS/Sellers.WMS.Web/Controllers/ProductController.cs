using System;
using System.Collections.Generic;
using System.Data;
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
    public class ProductController : BaseController
    {
        public ViewResult Index()
        {
            GetPermission();
            ViewData["toolbarButtons"] = BuildToolBarButtons();
            return View();
        }

        public override void GetPermission()
        {
            this.permissionAdd = this.IsAuthorized("Product.Add");
            this.permissionDelete = this.IsAuthorized("Product.Delete");
            this.permissionEdit = this.IsAuthorized("Product.Edit");
            this.permissionExport = this.IsAuthorized("Product.Export");
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

        public ActionResult Upload()
        {

            return View();
        }

        [HttpPost]
        public JsonResult Create(ProductType obj)
        {

            if (!IsFieldExist<ProductType>("SKU", obj.SKU, "-1"))
            {
                ProductImgType productImg = new ProductImgType();
                productImg.SKU = obj.SKU;
                productImg.MainSKU = obj.SKU;

                productImg.Img = ImageUtil.GetPictureData(Server.MapPath("~" + obj.ImgPath));
                productImg.ImgName = obj.SKU;

                ImageUtil.DrawImageRectRect(Server.MapPath("~" + obj.ImgPath),
                                            Server.MapPath("~" + Common.ImgPath) + obj.SKU + ".png", 310, 310);
                productImg.Src = obj.ImgPath = Common.ImgPath + obj.SKU + ".png";


                bool isOk = Save(obj);
                if (isOk)
                {
                    Save(productImg);

                }
                return Json(new { IsSuccess = isOk });
            }
            return Json(new { IsSuccess = false });
        }

        public JsonResult BatchImport(string filename)
        {
            List<ResultInfo> list = new List<ResultInfo>();
            filename = Server.MapPath("~" + filename);
            DataTable dt = Common.GetDataTable(filename);
            foreach (DataRow dr in dt.Rows)
            {
                ProductType product = new ProductType();
                product.SKU = dr["子SKU"].ToString().Trim();
                product.TempSKU = dr["主SKU"].ToString().Trim();
                product.Title = dr["商品名称"].ToString().Trim();
                product.ProductAttr = dr["特性"].ToString().Trim();
                product.Status = dr["状态"].ToString().Trim();
                product.Category = dr["分类"].ToString().Trim();
                product.Brand = dr["品牌"].ToString().Trim();
                product.Model = dr["型号"].ToString().Trim();
                product.Standard = dr["规格"].ToString().Trim();
                product.Weight = ZConvert.ToInt(dr["重量"].ToString().Trim());
                product.Price = ZConvert.ToDouble(dr["价格"].ToString().Trim());
                product.Long = ZConvert.ToInt(dr["长"].ToString().Trim());
                product.High = ZConvert.ToInt(dr["高"].ToString().Trim());
                product.Wide = ZConvert.ToInt(dr["宽"].ToString().Trim());

                if (!IsFieldExist<ProductType>("SKU", product.SKU, "-1"))
                {
                    Save(product);
                    list.Add(new ResultInfo { CreateOn = DateTime.Now, Key = product.SKU, Info = "商品信息格式正确，成功导入商品数据", Result = "导入成功" });
                }
                list.Add(new ResultInfo { CreateOn = DateTime.Now, Key = product.SKU, Info = "该SKU已经存在，请检查需要导入的数据", Result = "导入失败" });
            }
            Session["result"] = list;
            return Json(new {IsSuccess = true});
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductType GetById(int Id)
        {
            ProductType obj = Get<ProductType>(Id);
            return obj;
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            ProductType obj = GetById(id);
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(ProductType obj)
        {
            bool isOk = Update<ProductType>(obj);
            return Json(new { IsSuccess = isOk });
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            bool isOk = DeleteObj<ProductType>(id);
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
            IList<ProductType> objList = NSession.CreateQuery("from ProductType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<ProductType>();

            object count = NSession.CreateQuery("select count(Id) from ProductType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

    }
}

