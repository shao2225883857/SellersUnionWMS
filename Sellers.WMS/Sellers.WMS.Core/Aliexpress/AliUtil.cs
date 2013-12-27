using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sellers.WMS.Core.Aliexpress
{
    /// <summary>
    /// API网址
    /// 
    /// </summary>
    public class AliUtil
    {
        #region AliAPI名称
        /// <summary>
        /// 根据ID获得产品
        /// </summary>
        public const string ApifindAeProductById = "findAeProductById";
        /// <summary>
        /// 分页获得产品
        /// </summary>
        public const string ApifindProductInfoListQuery = "findProductInfoListQuery";
        /// <summary>
        /// 上传产品
        /// </summary>
        public const string ApipostAeProduct = "postAeProduct";
        /// <summary>
        /// 编辑产品
        /// </summary>
        public const string ApieditAeProduct = "editAeProduct";
        /// <summary>
        /// 计算运费
        /// </summary>
        public const string ApicalculateFreight = "calculateFreight";
        /// <summary>
        /// 根据订单Id获得订单那
        /// </summary>
        public const string ApifindOrderById = "findOrderById";
        /// <summary>
        /// 分页获得订单
        /// </summary>
        public const string ApifindOrderListQuery = "findOrderListQuery";
        /// <summary>
        /// 上传产品图片
        /// </summary>
        public const string ApiuploadTempImage = "uploadTempImage";

        /// <summary>
        /// 上传获得实际的属性设置
        /// </summary>
        public const string ApigetAttributesResultByCateId = "getAttributesResultByCateId";

        /// <summary>
        /// 查询留言
        /// </summary>
        public const string ApiqueryOrderMsgListByOrderId = "queryOrderMsgListByOrderId";

        /// <summary>
        /// 声明发货
        /// </summary>
        public const string ApisellerShipment = "sellerShipment";
        #endregion


        public const string IP = "https://gw.api.alibaba.com/openapi/";

        public const string IP2 = "http://gw.api.alibaba.com/fileapi/";

        public const string UrlRefreshToken = "param2/1/system.oauth2/refreshToken";

        public const string UrlGetToken = "http/1/system.oauth2/getToken";

        public const string Url = "param2/1/aliexpress.open/api.";

        public const string fieldAopSignature = "_aop_signature";

        public const string fieldAccessToken = "access_token";

        /// <summary>
        /// 获得签名
        /// </summary>
        /// <param name="urlPath">网址</param>
        /// <param name="paramDic">参数</param>
        /// <returns></returns>
        public static string Sign(string urlPath, Dictionary<string, string> paramDic, bool iscon = true)
        {
            if (iscon)
            {
                urlPath += "/" + ApiConfig.AliAppKey;
            }
            byte[] signatureKey = Encoding.UTF8.GetBytes(ApiConfig.AliAppSecret);
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, string> kv in paramDic)
            {
                list.Add(kv.Key + HttpUtility.UrlDecode(kv.Value));
            }
            list.Sort();
            string tmp = urlPath;
            foreach (string kvstr in list)
            {
                tmp = tmp + kvstr;
            }
            HMACSHA1 hmacshal = new HMACSHA1(signatureKey);
            hmacshal.ComputeHash(Encoding.UTF8.GetBytes(tmp));
            byte[] hash = hmacshal.Hash;
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper();
        }

        /// <summary>
        /// 获得参数的Url，更具首字母排序
        /// </summary>
        /// <param name="paramDic"></param>
        /// <returns></returns>
        public static string GetParamUrl(Dictionary<string, string> paramDic)
        {
            string tmp = "";
            foreach (KeyValuePair<string, string> kv in paramDic)
            {
                tmp += kv.Key + "=" + kv.Value + "&";
            }
            tmp = tmp.Trim('&');
            return tmp;
        }

        /// <summary>
        /// 发货连接请求
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="paramData"></param>
        /// <param name="isFile"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string PostWebRequest(string postUrl, string paramData, bool isFile = false, byte[] stream = null)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                // 这个可以是改变的，也可以是下面这个固定的字符串
                // 创建request对象
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(postUrl);

                webReq.ContentType = "application/x-www-form-urlencoded";
                Stream newStream = null;
                if (isFile)
                {
                    string boundary = "—————————7d930d1a850658";
                    webrequest.ContentType = "multipart/form-data; boundary=" + boundary;
                    webReq.ContentLength = stream.Length;
                    newStream = webReq.GetRequestStream();
                    newStream.Write(stream, 0, stream.Length);
                    newStream.Close();
                }
                else
                {
                    webReq.ContentLength = byteArray.Length;
                    newStream = webReq.GetRequestStream();
                    newStream.Write(byteArray, 0, byteArray.Length); //写入参数
                    newStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    StreamReader sr = new StreamReader(ex.Response.GetResponseStream(), Encoding.UTF8);
                    ret = sr.ReadToEnd();
                }
            }
            return ret;
        }


        #region 授权三部曲
        /// <summary>
        /// 去授权页面
        /// </summary>
        /// <returns></returns>
        public static string GetAuthUrl()
        {
            string url =
               "http://gw.api.alibaba.com/auth/authorize.htm?";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("client_id", ApiConfig.AliAppKey);
            dic.Add("site", "aliexpress");
            dic.Add("redirect_uri", HttpContext.Current.Request.Url.Authority+ ApiConfig.SystemReturnUrl);
            dic.Add("state", "sss");
            dic.Add("_aop_signature", Sign("", dic, false));
            return url + GetParamUrl(dic);
        }

        /// <summary>
        /// 由授权获得的code 申请 RefreshToken
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static AliShop GetRefreshToken(string code)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("grant_type", "authorization_code");
            dic.Add("client_id", ApiConfig.AliAppKey);
            dic.Add("client_secret", ApiConfig.AliAppSecret);
            dic.Add("redirect_uri", ApiConfig.SystemReturnUrl);
            dic.Add("code", code);
            dic.Add("need_refresh_token", "true");
            string c = PostWebRequest(IP + UrlGetToken + "/" + ApiConfig.AliAppKey, GetParamUrl(dic));
            AliShop shop = Newtonsoft.Json.JsonConvert.DeserializeObject<AliShop>(c);
            return shop;

        }

        /// <summary>
        /// 用refreshtoken申请获得 accessToken
        /// </summary>
        /// <param name="refreshtoken"></param>
        /// <returns></returns>
        public static string GetAccessToken(string refreshtoken)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("grant_type", "refresh_token");
            dic.Add("client_id", ApiConfig.AliAppKey);
            dic.Add("client_secret", ApiConfig.AliAppSecret);
            dic.Add("refresh_token", refreshtoken);
            dic.Add("_aop_signature", Sign(UrlRefreshToken, dic));
            string c = PostWebRequest(IP + UrlRefreshToken + "/" + ApiConfig.AliAppKey, GetParamUrl(dic));
            JToken token = (JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(c);
            return token["access_token"].ToString().Replace("\"", "");
        }
        #endregion


        /// <summary>
        /// 获得订单列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="pageIndex"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static AliOrderListType findOrderListQuery(string token, int pageIndex, AliOrderStatus state)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(fieldAccessToken, token);
            dic.Add("orderStatus", state.ToString());
            dic.Add("pageSize", "50");
            dic.Add("page", pageIndex.ToString());
            string c = PostWebRequest(IP + Url + ApifindOrderListQuery + "/" + ApiConfig.AliAppKey, GetParamUrl(dic));
            return JsonConvert.DeserializeObject<AliOrderListType>(c);
        }

        /// <summary>
        /// 根据订单Id 获得订单的详细信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="OId"></param>
        /// <returns></returns>
        public static AliOrderType findOrderById(string token, string OId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(fieldAccessToken, token);
            dic.Add("orderId", OId);
            string c = PostWebRequest(IP + Url + ApifindOrderById + "/" + ApiConfig.AliAppKey, GetParamUrl(dic));
            return JsonConvert.DeserializeObject<AliOrderType>(c);
        }

        /// <summary>
        /// 获得订单留言
        /// </summary>
        /// <param name="token"></param>
        /// <param name="OId"></param>
        /// <returns></returns>
        public static OrderMsgType[] findOrderMsgByOrderId(string token, string OId)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(fieldAccessToken, token);
            dic.Add("orderId", OId);
            string c = PostWebRequest(IP + Url + ApiqueryOrderMsgListByOrderId + "/" + ApiConfig.AliAppKey, GetParamUrl(dic));
            return Newtonsoft.Json.JsonConvert.DeserializeObject<OrderMsgType[]>(c);
        }
        /// <summary>
        /// 标记发货
        /// </summary>
        /// <param name="token"></param>
        /// <param name="orderExNo"></param>
        /// <param name="trackCode"></param>
        /// <param name="serviceName"></param>
        /// <param name="isALL"></param>
        /// <returns></returns>
        public static string sellerShipment(string token, string orderExNo, string trackCode, string serviceName, bool isALL = false)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(fieldAccessToken, token);
            dic.Add("serviceName", serviceName);
            dic.Add("logisticsNo", trackCode);
            if (isALL)
                dic.Add("sendType", "all");
            else
                dic.Add("sendType", "part");
            dic.Add("outRef", orderExNo);
            string c = PostWebRequest(IP + Url + "sellerShipment/" + ApiConfig.AliAppKey, GetParamUrl(dic));
            return c;
        }

        public static DateTime GetAliDate(string DateStr)
        {
            return new DateTime(Convert.ToInt32(DateStr.Substring(0, 4)), Convert.ToInt32(DateStr.Substring(4, 2)), Convert.ToInt32(DateStr.Substring(6, 2)), Convert.ToInt32(DateStr.Substring(8, 2)), Convert.ToInt32(DateStr.Substring(10, 2)), Convert.ToInt32(DateStr.Substring(12, 2)));
        }

    }
}
