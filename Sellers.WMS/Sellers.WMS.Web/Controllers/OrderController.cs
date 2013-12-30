using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Newtonsoft.Json;
using Sellers.WMS.Core.Aliexpress;
using Sellers.WMS.Domain;
using Sellers.WMS.Utils.Extensions;
using NHibernate;

namespace Sellers.WMS.Web.Controllers
{
    public class OrderController : BaseController
    {
        public ViewResult Index()
        {
            GetPermission();
            ViewData["toolbarButtons"] = BuildToolBarButtons();
            return View();
        }

        public override void GetPermission()
        {
            this.permissionAdd = this.IsAuthorized("Order.Add");
            this.permissionDelete = this.IsAuthorized("Order.Delete");
            this.permissionEdit = this.IsAuthorized("Order.Edit");
            this.permissionExport = this.IsAuthorized("Order.Export");
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
                    results = SynDataByAliexpress(st, et, account);
                    break;
                default:
                    isOk = false;
                    return Json(new { IsSuccess = isOk, Result = "该账户没有同步功能！" });
                    break;
            }
            if (results.Count > 0)
                Session["results"] = results;
            else
                isOk = false;

            return Json(new { IsSuccess = isOk, info = true });
        }

        private List<ResultInfo> SynDataByAliexpress(DateTime st, DateTime et, AccountType account)
        {
            List<ResultInfo> results = new List<ResultInfo>();
            string token = AliUtil.GetAccessToken(account.ApiToken);
            List<CountryType> countryTypes = NSession.CreateQuery("from CountryType").List<CountryType>().ToList();
            AliOrderListType aliOrderList = null;
            int page = 1;
            do
            {
                try
                {
                    aliOrderList = AliUtil.findOrderListQuery(token, page, AliOrderStatus.WAIT_SELLER_SEND_GOODS);
                    if (aliOrderList.totalItem != 0)
                    {

                        foreach (var o in aliOrderList.orderList)
                        {

                            bool isExist = IsFieldExist<OrderType>("OrderExNo", o.orderId.ToString(), "-1");

                            if (!isExist)
                            {
                                AliOrderType ot = AliUtil.findOrderById(token, o.orderId.ToString());
                                OrderType order = new OrderType
                                {
                                    IsMerger = 0,

                                    IsOutOfStock = 0,
                                    IsRepeat = 0,
                                    IsSplit = 0,
                                    Status = OrderStatusEnum.待处理.ToString(),
                                    IsPrint = 0
                                };
                                order.OrderNo = Common.GetOrderNo(NSession);
                                order.CurrencyCode = ot.orderAmount.currencyCode;
                                order.OrderExNo = ot.id.ToString();
                                order.Amount = ot.orderAmount.amount;
                                order.LogisticMode = o.productList[0].logisticsServiceName;
                                order.Account = account.AccountName;
                                order.GenerateOn = Common.GetAliDate(ot.gmtPaySuccess);
                                order.Platform = account.Platform;
                                CountryType country =
                                    countryTypes.Find(
                                        p => p.CountryCode.ToUpper() == ot.receiptAddress.country.ToUpper());
                                if (country != null)
                                {
                                    order.Country = country.ECountry;
                                }
                                else
                                {
                                    order.Country = ot.receiptAddress.country;
                                }

                                order.BuyerName = ot.buyerInfo.firstName + " " + ot.buyerInfo.lastName;
                                order.BuyerEmail = ot.buyerInfo.email;
                                order.BuyerId = CreateBuyer(order.BuyerName, order.BuyerEmail, order.Amount,
                                                            order.GenerateOn, order.Platform);
                                foreach (ProductList p in o.productList)
                                {
                                    order.BuyerMemo += p.memo;
                                }
                                OrderMsgType[] msgTypes = AliUtil.findOrderMsgByOrderId(token, order.OrderExNo);

                                foreach (OrderMsgType orderMsgType in msgTypes)
                                {
                                    order.BuyerMemo += "<br/>" + orderMsgType.senderName + "  " +
                                                       Common.GetAliDate(orderMsgType.gmtCreate).ToString("yyyy-MM-dd HH:mm:ss") +
                                                       ":" + orderMsgType.content + "";
                                }

                                order.TId = "";
                                order.AddressId = CreateAddress(ot.receiptAddress.contactPerson,
                                                                ot.receiptAddress.detailAddress + "  " + ot.receiptAddress.address2,
                                                                ot.receiptAddress.city, ot.receiptAddress.province,
                                                                country == null
                                                                    ? ot.receiptAddress.country
                                                                    : country.ECountry,
                                                                country == null
                                                                    ? ot.receiptAddress.country
                                                                    : country.CountryCode, ot.receiptAddress.phoneCountry + " " + ot.receiptAddress.phoneArea + " " + ot.receiptAddress.phoneNumber,
                                                                ot.receiptAddress.mobileNo, ot.buyerInfo.email,
                                                                ot.receiptAddress.zip, 0, NSession);
                                NSession.Save(order);
                                NSession.Flush();
                                foreach (ChildOrderList item in ot.childOrderList)
                                {
                                    string remark = "";
                                    if (item.productAttributes.Length > 0)
                                    {
                                        SkuListType skuList =
                                            JsonConvert.DeserializeObject<SkuListType>(
                                                item.productAttributes.Replace("\\", ""));
                                        foreach (SkuType skuType in skuList.sku)
                                        {
                                            remark += skuType.pName + ":" + skuType.pValue;
                                        }
                                    }
                                    CreateOrderPruduct(item.productId.ToString(), item.skuCode, item.productCount,
                                                       item.productName,
                                                       remark, item.initOrderAmt.amount,
                                                       "",
                                                       order.Id,
                                                       order.OrderNo, NSession);
                                }
                                NSession.Clear();
                                NSession.Update(order);
                                NSession.Flush();
                                results.Add(GetResult(order.OrderExNo, "", "导入成功"));
                            }
                            else
                            {
                                results.Add(GetResult(o.orderId.ToString(), "该订单已存在", "导入失败"));
                            }
                        }
                        page++;
                    }
                }
                catch (Exception)
                {
                    token = AliUtil.GetAccessToken(account.ApiToken);
                    continue;
                }

            } while (aliOrderList.totalItem > (page - 1) * 50);
            return results;
        }

        #region 获得返回的数据
        public ResultInfo GetResult(string key, string info, string result, string field1, string field2, string field3, string field4)
        {
            ResultInfo r = new ResultInfo();
            r.Field1 = field1;
            r.Field2 = field2;
            r.Field3 = field3;
            r.Field4 = field4;
            r.Key = key;
            r.Info = info;
            r.Result = result;
            r.CreateOn = DateTime.Now;
            return r;
        }

        public ResultInfo GetResult(string key, string info, string result)
        {
            return GetResult(key, info, result, "", "", "", "");
        }
        #endregion


        public int CreateBuyer(string name, string email, double amount, DateTime buyOn, string platform)
        {

            IList<OrderBuyerType> list = NSession.CreateQuery(" from OrderBuyerType where BuyerName=:p and Platform=:p2").SetString("p", name).SetString("p2", platform.ToString()).List<OrderBuyerType>();
            OrderBuyerType buyer = new OrderBuyerType();
            if (list.Count > 0)
            {
                buyer = list[0];
                buyer.BuyCount += 1;
                buyer.BuyAmount += amount;
                buyer.LastBuyOn = buyOn;
            }
            else
            {
                buyer = new OrderBuyerType();
                buyer.BuyerName = name;
                buyer.BuyerEmail = email;
                buyer.FristBuyOn = buyOn;
                buyer.BuyCount = 1;
                buyer.BuyAmount = amount;
                buyer.LastBuyOn = buyOn;
                buyer.Platform = platform;
            }
            NSession.SaveOrUpdate(buyer);
            NSession.Flush();
            return buyer.Id;
        }


        public void CreateOrderPruduct(string exSKU, string sku, int qty, string name, string remark, double price, string url, int oid, string orderNo, ISession NSession)
        {
            OrderProductType product = new OrderProductType();
            product.ExSKU = exSKU;
            if (sku != null)
                product.SKU = sku.Trim();
            product.Qty = qty;
            product.Price = price;
            product.Title = name;
            product.Url = url;
            product.OId = oid;
            product.OrderNo = orderNo;
            product.Remark = remark;
            // CreateOrderPruduct(product, NSession);

        }



        public void CreateOrderPruduct(OrderProductType product, ISession NSession)
        {
            IList<ProductComposeType> products = NSession.CreateQuery("from ProductComposeType").List<ProductComposeType>();
            if (product.SKU == null)
                product.SKU = "";
            if (product.SKU.IndexOf("+") != -1)
            {
                int qty = product.Qty;
                foreach (string fo in product.SKU.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries))
                {

                    product.Qty = qty;
                    product.Id = 0;
                    if (fo != null)
                        product.SKU = fo.Trim();
                    // GetItem(product, NSession);
                    NSession.Save(product);
                    NSession.Flush();
                    NSession.Clear();
                    //SplitProduct(product, NSession, products);
                }
            }
            else
            {
                //GetItem(product, NSession);
                NSession.Save(product);
                NSession.Flush();
                // SplitProduct(product, NSession, products);
            }
        }

        public int CreateAddress(string addressee, string street, string city, string province, string country, string countryCode, string tel, string phone, string email, string postcode, int buyerID, ISession NSession)
        {
            try
            {

                OrderAddressType address = new OrderAddressType();
                address.Street = street;
                address.Tel = tel;
                address.City = city;
                address.Province = province;
                address.PostCode = postcode;
                address.Email = email;
                address.Country = country;
                address.CountryCode = countryCode;
                address.Phone = phone;
                address.Addressee = addressee;
                address.BId = buyerID;
                NSession.Save(address);
                NSession.Flush();
                return address.Id;
            }
            catch (Exception)
            {

                throw;
            }

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
            return View(obj);
        }

        [HttpPost]
        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Edit(OrderType obj)
        {
            bool isOk = Update<OrderType>(obj);
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

    }
}

