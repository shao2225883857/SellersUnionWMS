using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using NHibernate.Criterion;
using Newtonsoft.Json;
using Sellers.WMS.Core.Aliexpress;
using Sellers.WMS.Domain;
using Sellers.WMS.Utils.Extensions;
using NHibernate;

namespace Sellers.WMS.Web.Controllers
{
    public class OrderController : BaseController
    {
        private bool permissionPrint = false;
        private bool permissionSetLogistics = false;

        public ViewResult Index()
        {
            GetPermission();
            ViewData["toolbarButtons"] = BuildToolBarButtons();
            return View();
        }

        public ViewResult OrderSend()
        {
            return View();
        }

        public ViewResult OrderExport()
        {
            return View();
        }

        public ViewResult UnHandIndex()
        {
            GetPermission();
            ViewData["toolbarButtons"] = BuildToolBarButtons(1);
            return View();
        }

        public override void GetPermission()
        {
            this.permissionAdd = this.IsAuthorized("Order.Add");
            this.permissionDelete = this.IsAuthorized("Order.Delete");
            this.permissionEdit = this.IsAuthorized("Order.Edit");
            this.permissionExport = this.IsAuthorized("Order.Export");
            this.permissionPrint = this.IsAuthorized("Order.Print");
            this.permissionSetLogistics = this.IsAuthorized("Order.ShowSetLogistics ");
        }

        /// <summary>  
        /// 加载工具栏  
        /// </summary>  
        /// <returns>工具栏HTML</returns>  
        public override string BuildToolBarButtons(int t = 0)
        {
            StringBuilder sb = new StringBuilder();
            string linkbtn_template =
                "<a id=\"a_{0}\" class=\"easyui-linkbutton\" style=\"float:left\"  plain=\"true\" href=\"javascript:;\" icon=\"{1}\"  {2} title=\"{3}\" onclick='{5}'>{4}</a>";
            sb.Append(
                "<a id=\"a_refresh\" class=\"easyui-linkbutton\" style=\"float:left\"  plain=\"true\" href=\"javascript:;\" icon=\"icon-reload\"  title=\"重新加载\"  onclick='refreshClick()'>刷新</a> ");
            sb.Append("<div class='datagrid-btn-separator'></div> ");
            sb.Append(string.Format(linkbtn_template, "add", "icon-add", permissionAdd ? "" : "disabled=\"True\"", "添加",
                                    "添加", "addClick()"));
            sb.Append(string.Format(linkbtn_template, "edit", "icon-edit", permissionEdit ? "" : "disabled=\"True\"",
                                    "修改", "修改", "editClick()"));
            if (t == 1)
            {
                sb.Append(string.Format(linkbtn_template, "SetLogistics", "icon-redo",
                                        permissionSetLogistics ? "" : "disabled=\"True\"", "设置发货方式", "设置发货方式",
                                        "showSetLogistics()"));
                sb.Append(string.Format(linkbtn_template, "delete", "icon-remove",
                                        permissionDelete ? "" : "disabled=\"True\"", "删除", "删除", "delClick()"));
            }

            if (t == 0)
                sb.Append(string.Format(linkbtn_template, "print", "icon-print", permissionPrint ? "" : "disabled=\"True\"", "打印订单", "打印订单", "printOrder()"));
            sb.Append("<div class='datagrid-btn-separator'></div> ");
            sb.Append("<a href=\"#\" class='easyui-menubutton' " + (permissionExport ? "" : "disabled='True'") + " data-options=\"menu:'#dropdown',iconCls:'icon-undo'\">导出</a>");
            return sb.ToString();
        }

        public JsonResult SetLogistics(string ids, string l)
        {
            int count = NSession.CreateQuery("update OrderType set LogisticMode=:p1 where Id in(" + ids + ")").SetString("p1", l).ExecuteUpdate();
            if (count > 0)
                return Json(new { IsSuccess = true });
            else
                return Json(new { IsSuccess = false });
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult OrderImport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ImportData(string filename, int aId)
        {
            bool isOk = true;
            return Json(new { IsSuccess = isOk });
        }

        [HttpPost]
        public JsonResult SynData(DateTime st, DateTime et, string Platform, int Account)
        {
            List<ResultInfo> results = new List<ResultInfo>();
            AccountType account = Get<AccountType>(Account);
            bool isOk = true;

            switch (Platform)
            {
                case "Aliexpress":
                    isOk = true;
                    results = OrderHelper.SynDataByAliexpress(st, et, account, NSession);
                    break;
                default:
                    isOk = false;
                    return Json(new { IsSuccess = isOk, Result = "该账户没有同步功能！" });
                    break;
            }
            if (results.Count > 0)
                Session["result"] = results;
            else
                isOk = false;

            return Json(new { IsSuccess = isOk, info = true });
        }

        [HttpPost]
        public JsonResult Create(OrderType obj)
        {
            bool isOk = Save(obj);
            return Json(new { IsSuccess = isOk });
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public OrderType GetById(int Id)
        {
            OrderType obj = Get<OrderType>(Id);
            return obj;
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(int id)
        {
            OrderType obj = GetById(id);
            obj.Address = Get<OrderAddressType>(obj.AddressId);
            ViewData["id"] = id;
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(OrderType obj)
        {

            OrderType order = GetById(obj.Id);
            order.Address = Get<OrderAddressType>(order.AddressId);
            order.OrderExNo = obj.OrderExNo;
            order.TId = obj.TId;
            order.Platform = obj.Platform;
            order.Account = obj.Account;
            order.SellerMemo = obj.SellerMemo;
            order.BuyerEmail = obj.BuyerEmail;
            order.BuyerName = obj.BuyerName;
            order.TrackCode = obj.TrackCode;
            order.Weight = obj.Weight;
            order.Country = obj.Address.Country;
            order.BuyerMemo = obj.BuyerMemo;
            order.Amount = obj.Amount;
            order.CurrencyCode = obj.CurrencyCode;
            order.LogisticMode = obj.LogisticMode;

            order.Address.Addressee = obj.Address.Addressee;
            order.Address.Street = obj.Address.Street;
            order.Address.County = obj.Address.County;
            order.Address.Country = obj.Address.Country;
            order.Address.CountryCode = obj.Address.CountryCode;
            order.Address.City = obj.Address.City;
            order.Address.Province = obj.Address.Province;

            order.Address.Email = obj.Address.Email;
            order.Address.PostCode = obj.Address.PostCode;
            order.Address.Tel = obj.Address.Tel;
            order.Address.Phone = obj.Address.Phone;

            bool isOk = Update<OrderType>(order);
            isOk = Update<OrderAddressType>(order.Address);
            return Json(new { IsSuccess = isOk });
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            bool isOk = DeleteObj<OrderType>(id);
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
            IList<OrderType> objList = NSession.CreateQuery("from OrderType " + where + orderby)
                .SetFirstResult(rows * (page - 1))
                .SetMaxResults(rows)
                .List<OrderType>();
            string ids = "";
            foreach (var item in objList)
            {
                ids += item.Id + ",";
            }
            ids = ids.Trim(',');
            if (ids.Length > 0)
            {
                IList<OrderProductType> products = NSession.CreateQuery(" from OrderProductType where OId in(" + ids + ")").List<OrderProductType>();
                foreach (var item in objList)
                {
                    item.ProductList = products.Where(x => x.OId == item.Id).ToList<OrderProductType>();
                }

            }
            object count = NSession.CreateQuery("select count(Id) from OrderType " + where).UniqueResult();
            return Json(new { total = count, rows = objList });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetOrderBySend(string o)
        {
            List<OrderType> orderTypes = GetList<OrderType>("OrderNo", o, "");
            StringBuilder html = new StringBuilder();
            if (orderTypes.Count > 0)
            {
                if (orderTypes[0].Status != "已发货")
                    return Json(new { IsSuccess = true, Result = string.Format("订单：{0} ，订单状态：{1} ，发货方式：{2} ", orderTypes[0].OrderNo, orderTypes[0].Status, orderTypes[0].LogisticMode) });
                else
                    return Json(new { IsSuccess = false, Result = string.Format("订单：{0}，已经发货！无法再次扫描！", orderTypes[0].OrderNo) });
            }
            return Json(new { IsSuccess = false, Result = string.Format("订单：{0}，不存在！", o) });
        }

        /// <summary>
        /// 发货扫描
        /// </summary>
        /// <param name="o">订单编号</param>
        /// <param name="l">追踪条码</param>
        /// <param name="w">重量</param>
        /// <param name="m">发货方式</param>
        /// <param name="c">仓库Id</param>
        /// <returns></returns>
        public JsonResult ScanBySend(string o, string l, int w, string m, string c)
        {
            List<OrderType> orderTypes = GetList<OrderType>("OrderNo", o, "");
            StringBuilder html = new StringBuilder();
            if (orderTypes.Count > 0)
            {
                orderTypes[0].Status = "已发货";
                orderTypes[0].ScanOn = DateTime.Now;
                orderTypes[0].ScanBy = CurrentUser.Realname;
                orderTypes[0].LogisticMode = m != "" ? m : orderTypes[0].LogisticMode;
                orderTypes[0].Weight = w;
                orderTypes[0].TrackCode = l;
                SaveOrUpdate(orderTypes[0]);
                NSession.Flush();
                //
                //仓库出货
                //

                html.AppendFormat("订单：{0} ，已经发货了，发货方式为{1}，重量：{2} ，追踪条码：{3}", orderTypes[0].OrderNo, orderTypes[0].LogisticMode, orderTypes[0].Weight, orderTypes[0].TrackCode);

            }
            return Json(new { IsSuccess = true, Result = html.ToString() });
        }

        /// <summary>
        /// 订单验证
        /// </summary>
        /// <param name="ids">订单id集合</param>
        /// <returns></returns>
        public JsonResult ValiOrder(string ids)
        {
            List<OrderType> orderTypes =
                NSession.CreateQuery("from OrderType where Id in (" + ids + ")").List<OrderType>().ToList();
            List<CountryType> countrys = GetAll<CountryType>();
            List<CurrencyType> currencys = GetAll<CurrencyType>();
            foreach (var orderType in orderTypes)
            {
                OrderHelper.ValiOrder(orderType, countrys, currencys, NSession);
            }
            return Json(new { IsSuccess = true });
        }

        public ActionResult ExportOrder(DateTime st, DateTime et)
        {
            string sql =
                @"select '' as '记录号',  O.OrderNo,OrderExNo,CurrencyCode,Amount,OrderCurrencyCode,OrderFees,OrderCurrencyCode2,OrderFees2,TId,BuyerName,BuyerEmail,LogisticMode,Country,O.Weight,TrackCode,OP.SKU,OP.Qty,p.Price,OP.Standard,0.00 as 'TotalPrice',O.Freight,O.CreateOn,O.ScanningOn,O.ScanningBy,O.Account,cast(O.IsSplit as nvarchar) as '拆分',cast(O.IsRepeat as nvarchar) as '重发',O.BuyerName   from Orders O left join OrderProducts OP ON O.Id =OP.OId 
left join Products P On OP.SKU=P.SKU ";
            string sql2 = "";
            //            if (t == 1)
            //            {
            //                sql = @"select TrackCode as '跟踪号',OA.City as '收件人城市名',OA.Addressee as '收件人全名',oa.Street+' '+oa.City+' '+OA.Province+' '+OA.Country+' '+OA.PostCode as '收件人详细地址',oa.Phone+'('+oa.Tel+')' as '收件人电话','' as 寄件人详细地址及姓名,OP.Title as '物品名称',OP.Qty as '数量',o.weight as '重量',10 as '申报价值','China' as '原产地' from Orders O
            //                      left join OrderProducts OP on O.Id=OP.OId
            //                      left join OrderAddress OA on O.AddressId=OA.Id";
            //                //跟踪号	物品中文名称	物品英文名称(不能超过50个字符）	数量	单件重量	单价	原产地
            //            }

            sql = @"select  TrackCode as '运单码',C.CCountry as '寄达国家（中文）',O.Country as '寄达国家（英文）',OA.Province as '州名',OA.City as '城市名',
isnull(oa.Street,'')+','+isnull(oa.City,'')+','+isnull(OA.Province,'')+','+isnull(OA.Country,'')+','+isnull(OA.PostCode,'') as '收件人详细地址',OA.Addressee as '收件人姓名',isnull(oa.Phone,'')+'('+isnull(oa.Tel,'')+')' as '收件人电话','High-tech zone，Juxian Road 399, Building B1 20th, Ningbo, ZheJiang,China' as '寄件人详细地址（英文）','Answer' as '寄件人姓名','0574-27903940' as '寄件人电话','1' as '内件类型代码' from Orders O
left join OrderAddress OA on O.AddressId=OA.Id
left join Countrys C On O.Country=C.ECountry";
            sql2 = @"select TrackCode as '跟踪号','物品' as '物品中文名称',OP.Title as '物品英文名称(不能超过50个字符）',OP.Qty as '数量',o.weight as '单件重量',10 as '单价','China' as '原产地' from Orders O
left join OrderProducts OP on O.Id=OP.OId
left join OrderAddress OA on O.AddressId=OA.Id";
            //运单码	寄达国家（中文）	寄达国家（英文）	州名	城市名	收件人详细地址	收件人姓名	收件人电话	寄件人详细地址（英文）	寄件人姓名	寄件人电话	内件类型代码
            //
            //
            sql += " where  Status='已发货' and  ScanOn between '" + st.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + et.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            DataSet ds = GetOrderExport(sql);
            if (sql2.Length > 5)
            {
                sql2 += " where  O.Status='已发货' and  ScanOn between '" + st.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + et.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                DataSet ds2 = GetOrderExport(sql2);
                DataTable dt = ds2.Tables[0].Clone();
                foreach (DataRow item in ds2.Tables[0].Rows)
                {
                    if (item["物品英文名称(不能超过50个字符）"].ToString().Length > 48)
                    {
                        item["物品英文名称(不能超过50个字符）"] = item["物品英文名称(不能超过50个字符）"].ToString().Substring(0, 45);
                    }
                    dt.Rows.Add(item.ItemArray);
                }
                dt.TableName = "Sheet2";
                ds.Tables.Add(dt);
                ds.Tables[0].TableName = "Sheet1";

            }

            // 设置编码和附件格式 
            Session["ExportDown"] = ExcelHelper.GetExcelXml(ds);
            return Json(new { IsSuccess = true });
        }

        private DataSet GetOrderExport(string sql)
        {
            var ds = new DataSet();
            IDbCommand command = NSession.Connection.CreateCommand();
            command.CommandText = sql + " order by O.OrderExNo,O.OrderNo asc";
            var da = new SqlDataAdapter(command as SqlCommand);
            da.Fill(ds);
            return ds;

        }

        [HttpGet]
        public ActionResult ExportDown(string Id)
        {
            string str = "";
            object sb = Session["ExportDown"];
            if (sb != null)
            {
                str = sb.ToString();
            }
            if (Id == null)
            {
                System.Web.HttpContext.Current.Response.ContentType = "text/plain";
                System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                System.Web.HttpContext.Current.Response.Charset = "gb2312";
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=zm.txt");
                return File(Encoding.UTF8.GetBytes(str), "attachment;filename=zm.txt");
            }
            else
            {
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                System.Web.HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
                System.Web.HttpContext.Current.Response.Charset = "gb2312";
                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition",
                                                                     "attachment;filename=" +
                                                                     DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
                return File(Encoding.UTF8.GetBytes(str),
                            "attachment;filename=" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
            }
        }
    }
}

