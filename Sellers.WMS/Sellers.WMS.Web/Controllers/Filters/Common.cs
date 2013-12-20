using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NHibernate;
using Sellers.WMS.Data.Repository;
using Sellers.WMS.Domain;

namespace Sellers.WMS.Web
{
    public class Common
    {
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
                NSession.Delete(" from CurrencyType where CurrencySign='" + c.CurrencySign + "'");
                NSession.Flush();
                string str = dr["fBuyPrice"].ToString();
                if (string.IsNullOrEmpty(str))
                {
                    str = "0";
                }
                c.CurrencyValue = Math.Round(Convert.ToDecimal(str) / 100, 5);
                c.UpdateOn = DateTime.Now;
                NSession.Save(c);
                NSession.Flush();
            }
        }
    }
}