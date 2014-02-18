﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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


        public string BuildCheckBoxList(string name, List<DataDictionaryDetailType> dics)
        {
            string ck_template =
                "<label class='type-check-box-label'><input name='{0}' type='checkbox' id='{0}' value='{1}'><span>{2}</span></label>";
            StringBuilder sb = new StringBuilder();
            foreach (DataDictionaryDetailType obj in dics)
            {
                sb.AppendLine(string.Format(ck_template, name, obj.DicValue, obj.FullName));
            }
            return sb.ToString();
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

        public ViewResult AutoCreate()
        {
            ViewData["Size"] = BuildCheckBoxList("Size", GetList<DataDictionaryDetailType>("DicCode", "Size", ""));
            ViewData["Color"] = BuildCheckBoxList("Color", GetList<DataDictionaryDetailType>("DicCode", "Color", ""));
            return View();
        }


        public ActionResult Upload()
        {
            return View();
        }

        public ActionResult BatchUpdate()
        {
            return View();
        }

        public JsonResult DoBatchUpdate(string data, string key)
        {
            string[] datas = data.Replace("\r", "").Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string sqltemp = "Update ProductType set {0}={1} where SKU='{3}'";
            Type type = typeof(ProductType);

            PropertyInfo f = type.GetProperty(key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
  | BindingFlags.Static);
            List<ResultInfo> results = new List<ResultInfo>();
            foreach (string s in datas)
            {
                string[] cels = s.Replace("\t", " ").Replace("  ", " ").Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (cels.Length == 2)
                {
                    List<ProductType> products = GetList<ProductType>("SKU", cels[0].Trim(), "");
                    if (products.Count > 0)
                    {

                        foreach (ProductType productType in products)
                        {
                            object obj = cels[1].Trim();
                            f.SetValue(productType, obj, null);
                            Update(productType);
                        }
                        results.Add(new ResultInfo { Key = cels[0], Info = "修改完成", CreateOn = DateTime.Now });
                    }
                    else
                        results.Add(new ResultInfo { Key = cels[0], Info = "没有找到对应的产品数据", CreateOn = DateTime.Now });
                }
                else
                    results.Add(new ResultInfo { Key = s, Info = "格式错误", CreateOn = DateTime.Now });

            }
            Session["result"] = results;
            return Json(new { IsSuccess = true });
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AutoCreate(ProductType obj, string[] Size, string[] Color, string Category2)
        {
            ProductCategoryType c1 = Get<ProductCategoryType>(ZConvert.ToInt(obj.Category));
            ProductCategoryType c2 = Get<ProductCategoryType>(ZConvert.ToInt(Category2));
            List<DataDictionaryDetailType> colors = GetList<DataDictionaryDetailType>("DicCode", "Color", "");
            List<DataDictionaryDetailType> sizes = GetList<DataDictionaryDetailType>("DicCode", "Size", "");
            string sku = c1.Code + c2.Code;
            string num = Common.GetNo(NSession, sku);
            while (num.Length<3)
            {
                num = "0" + num;
            }
            sku = sku + num;
            obj.TempSKU = sku;
            obj.Category = c2.Name;
            for (int i = 0; i < Size.Length; i++)
            {
                DataDictionaryDetailType size = sizes.Find(p => p.DicValue == Size[i]);
                for (int j = 0; j < Color.Length; j++)
                {
                    DataDictionaryDetailType color = colors.Find(p => p.DicValue == Color[j]);
                    obj.SKU = obj.TempSKU + size.DicValue + color.DicValue;
                    obj.Standard = color.FullName + "  " + size.FullName;
                    if (!IsFieldExist<ProductType>("SKU", obj.SKU, "-1"))
                    {
                        obj.Id = 0;
                        NSession.Clear();
                        bool isOk = Save(obj);
                    }
                }
            }
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create(ProductType obj)
        {
            if (!IsFieldExist<ProductType>("SKU", obj.SKU, "-1"))
            {
                ProductImgType productImg = new ProductImgType();
                productImg.SKU = obj.SKU;
                productImg.MainSKU = obj.TempSKU;
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
            return Json(new { IsSuccess = false, Result = "SKU已经存在!" });


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
            return Json(new { IsSuccess = true });
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

        public JsonResult GetListBySKU(string q)
        {
            IList<ProductType> objList = NSession.CreateQuery("from ProductType where Status <>'停产' and SKU like '%" + q + "%'")
                .SetFirstResult(0)
                .SetMaxResults(20)
                .List<ProductType>();

            return Json(new { total = objList.Count, rows = objList });
        }

    }
}

