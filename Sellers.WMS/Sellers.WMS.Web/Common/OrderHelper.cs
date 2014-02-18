using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using NHibernate;
using Newtonsoft.Json;
using Sellers.WMS.Core.Aliexpress;
using Sellers.WMS.Domain;
using Sellers.WMS.Utils.Extensions;
using Sellers.WMS.Web.Controllers;

namespace Sellers.WMS.Web
{
    public class OrderHelper
    {
        #region 是否存在订单 public static bool IsExist(string OrderExNo)
        public static bool HasExist(string OrderExNo, ISession NSession, string Account = null)
        {
            object obj = 0;
            if (string.IsNullOrEmpty(Account))
                obj =
                   NSession.CreateQuery("select count(Id) from OrderType where OrderExNo=:p").SetString("p", OrderExNo)
                       .UniqueResult();
            else
                obj =
                     NSession.CreateQuery("select count(Id) from OrderType where OrderExNo=:p and Account=:p2").SetString("p", OrderExNo).SetString("p2", Account)
                         .UniqueResult();
            if (Convert.ToInt32(obj) > 0)
                return true;
            return false;
        }
        #endregion

        public static List<ResultInfo> SynDataByAliexpress(DateTime st, DateTime et, AccountType account, ISession NSession)
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
                            bool isExist = HasExist(o.orderId.ToString(), NSession);
                            if (!isExist)
                            {
                                AliOrderType ot = AliUtil.findOrderById(token, o.orderId.ToString());
                                OrderType order = new OrderType
                                {
                                    IsMerger = 0,
                                    Enabled = 1,
                                    IsOutOfStock = 0,
                                    IsRepeat = 0,
                                    IsSplit = 0,
                                    Status = OrderStatusEnum.待处理.ToString(),
                                    IsPrint = 0,
                                    ScanOn = DateTime.Now
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
                                                            order.GenerateOn, order.Platform, NSession);
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
                                            remark += skuType.pName + ":" + skuType.pValue + " ";
                                        }
                                    }
                                    CreateOrderPruduct(item.productId.ToString(), item.skuCode, item.productCount,
                                                       item.productName,
                                                       remark, item.initOrderAmt.amount,
                                                       "",
                                                       order.Id,
                                                       order.OrderNo, "http://i01.i.aliimg.com/wsphoto/v0/" + item.productId.ToString() + "_1/sku.jpg_50x50.jpg", NSession);
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
        public static ResultInfo GetResult(string key, string info, string result, string field1, string field2, string field3, string field4)
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

        public static ResultInfo GetResult(string key, string info, string result)
        {
            return GetResult(key, info, result, "", "", "", "");
        }
        #endregion


        public static int CreateBuyer(string name, string email, double amount, DateTime buyOn, string platform, ISession NSession)
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


        public static void CreateOrderPruduct(string exSKU, string sku, int qty, string name, string remark, double price, string url, int oid, string orderNo, string imgUrl, ISession NSession)
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
            product.ImgUrl = imgUrl;
            CreateOrderPruduct(product, NSession);
        }

        public static void CreateOrderPruduct(OrderProductType product, ISession NSession)
        {
            IList<ProductType> ps = NSession.CreateQuery("from ProductType where SKU='" + product.SKU + "'").List<ProductType>();
            if (ps.Count > 0)
            {
                if (ps[0].PType == 1)
                {
                    List<ProductComposeType> productComposeTypes = NSession.CreateQuery("from ProductComposeType where SKU='" + product.SKU + "' ").List<ProductComposeType>().ToList();
                    int qty = product.Qty;
                    int id = product.Id;
                    foreach (ProductComposeType productComposeType in productComposeTypes)
                    {
                        ProductType p = NSession.Get<ProductType>(productComposeType.SrcPId);
                        product.SKU = productComposeType.SrcSKU;
                        product.Qty = productComposeType.SrcQty * qty;
                        product.Id = 0;
                        product.Standard = p.Standard;
                        NSession.Clear();
                        NSession.Save(product);
                        NSession.Flush();
                    }
                }
                else
                {
                    product.Standard = ps[0].Standard;
                    NSession.Clear();
                    NSession.Save(product);
                    NSession.Flush();
                }
            }

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
                    //GetItem(product, NSession);
                    NSession.Save(product);
                    NSession.Flush();
                    NSession.Clear();
                    //SplitProduct(product, NSession, products);
                }
            }
            else
            {
                GetItem(product, NSession);
                NSession.Save(product);
                NSession.Flush();
            }
        }

        public static void GetItem(OrderProductType item, ISession NSession)
        {
            IList<ProductType> ps = NSession.CreateQuery("from ProductType where sku='" + item.SKU + "'").List<ProductType>();
            if (ps.Count > 0)
            {
                item.Standard = ps[0].Standard;
            }
        }

        public static int CreateAddress(string addressee, string street, string city, string province, string country, string countryCode, string tel, string phone, string email, string postcode, int buyerID, ISession NSession)
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

        public static void ValiOrder(OrderType orderType, List<CountryType> countrys, List<CurrencyType> currencys, ISession NSession)
        {
            if (countrys.FindIndex(p => p.ECountry == orderType.Country) == -1)
                orderType.ValiInfo = "国家不符";
            if (currencys.FindIndex(p => p.CurrencySign == orderType.CurrencyCode) == -1)
                orderType.ValiInfo = "货币不符";
            List<OrderProductType> orderProductTypes = NSession.CreateQuery("from OrderProductType where OId=:p1").SetInt32("p1", orderType.Id).List
                   <OrderProductType>().ToList();
            List<ProductType> products = new List<ProductType>();
            foreach (OrderProductType orderProductType in orderProductTypes)
            {
                if (string.IsNullOrEmpty(orderProductType.Standard))
                {
                    ProductType product = products.Find(p => p.SKU == orderProductType.SKU);
                    if (product != null)
                    {

                    }
                    else
                    {
                        List<ProductType> ps = NSession.CreateQuery("from ProductType where SKU=:p1").SetString("p1", orderProductType.SKU).
                              List<ProductType>().ToList();
                        if (ps.Count > 0)
                        {

                        }
                    }
                    orderType.ValiInfo = "SKU不符";
                }
            }
        }
    }

}