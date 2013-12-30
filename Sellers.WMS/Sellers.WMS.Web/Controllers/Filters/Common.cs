using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web;
using NHibernate;
using Sellers.WMS.Data.Repository;
using Sellers.WMS.Domain;
using Sellers.WMS.Web.Controllers;
using Sellers.WMS.Web.Controllers.Filters;

namespace Sellers.WMS.Web
{
    public class Common
    {

        public const string ImgPath = "/Product/Images/";

        public static object obj1 = new object();

        public static string GetOrderNo(ISession NSession)
        {
            lock (obj1)
            {
                string result = string.Empty;
                NSession.Clear();
                IList<SerialNumberType> list = NSession.CreateQuery(" from SerialNumberType where Code=:p").SetString("p", "OrderNo").List<SerialNumberType>();
                if (list.Count > 0)
                {
                    list[0].BeginNo = list[0].BeginNo + 1;
                    NSession.Update(list[0]);
                    NSession.Flush();
                    return list[0].BeginNo.ToString();
                }
                return "";
            }
        }

        public static DateTime GetAliDate(string DateStr)
        {
            return new DateTime(Convert.ToInt32(DateStr.Substring(0, 4)), Convert.ToInt32(DateStr.Substring(4, 2)), Convert.ToInt32(DateStr.Substring(6, 2)), Convert.ToInt32(DateStr.Substring(8, 2)), Convert.ToInt32(DateStr.Substring(10, 2)), Convert.ToInt32(DateStr.Substring(12, 2)));
        }

        public static DataTable GetDataTable(string fileName)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";" +
            "Extended Properties='Excel 8.0;IMEX=1'";
            DataSet ds = new DataSet();
            OleDbDataAdapter oada = new OleDbDataAdapter("select * from [" + ExcelSheetName(fileName)[0] + "]", strConn);
            oada.Fill(ds);
            return ds.Tables[0];
        }

        public static ArrayList ExcelSheetName(string filepath)
        {
            ArrayList al = new ArrayList();
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable sheetNames = conn.GetOleDbSchemaTable
            (System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            conn.Close();
            foreach (DataRow dr in sheetNames.Rows)
            {
                al.Add(dr[2]);
            }
            return al;
        }

        public static void UpdateCurrency()
        {
            ISession NSession = NhbHelper.OpenSession();
            cn.com.webxml.webservice.ForexRmbRateWebService server = new cn.com.webxml.webservice.ForexRmbRateWebService();
            DataSet ds = server.getForexRmbRate();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                CurrencyType c = new CurrencyType();
                c.CurrencySign = dr["Symbol"].ToString();
                c.CurrencyName = dr["Name"].ToString();
                NSession.Delete(" from CurrencyType where IsAutoUpdate=1 and CurrencySign='" + c.CurrencySign + "'");
                NSession.Flush();
                string str = dr["fBuyPrice"].ToString();
                if (string.IsNullOrEmpty(str))
                {
                    str = "0";
                }
                c.IsAutoUpdate = 1;
                c.CurrencyValue = Math.Round(Convert.ToDecimal(str) / 100, 5);
                c.UpdateOn = DateTime.Now;
                NSession.Save(c);
                NSession.Flush();
            }
        }
        /// <summary>
        /// Create or Set Cookies Values
        /// </summary>
        /// <param name="Obj">[0]:Name,[1]:Value</param>
        public static void CreateCookies(string u, string p, int t)
        {


            try
            {
                HttpCookie cookie = new HttpCookie("sellersUser")
                {
                    Expires = DateTime.Now.AddDays(t),
                };
                cookie.Value = u + "&" + p;
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            catch
            {


            }
        }

        /// <summary>
        /// Get Cookies Values
        /// </summary>
        /// <param name="name">Cookies的Name</param>
        /// <returns></returns>
        public static string GetCookies()
        {
            try
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["sellersUser"];
                return cookie.Value;
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Clear Cookies Values
        /// </summary>
        /// <param name="name">Cookies的Name</param>
        public static void ClearCookies()
        {
            HttpCookie cookie = new HttpCookie("sellersUser")
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            System.Web.HttpContext.Current.Session["user"] = null;
        }

        public static bool LoginByUser(string p, string u, ISession NSession)
        {

            IList<UserType> list = NSession.CreateQuery(" from  UserType where Username=:p1 and Password=:p2").SetString("p1", p).SetString("p2", u).List<UserType>();
            if (list.Count > 0)
            {   //登录成功
                UserType user = list[0];
                user.LastVisit = DateTime.Now;
                NSession.Update(user);
                user.PermissionList =
                    NSession.CreateQuery("from PermissionScopeType where ResourceCategory=:p1 and ResourceId=:p2").
                        SetString("p1", ResourceCategoryEnum.User.ToString()).SetInt32("p2", user.Id).List
                        <PermissionScopeType>();
                NSession.Flush();
                //GetPM(user, NSession);
                System.Web.HttpContext.Current.Session["user"] = user;
                return true;
            }
            return false;
        }
    }
}