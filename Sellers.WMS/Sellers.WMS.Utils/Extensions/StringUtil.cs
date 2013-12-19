using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

using System.Xml.Serialization;

using NHibernate;
using Newtonsoft.Json;

namespace Sellers.WMS.Utils.Extensions
{
    public static class StringUtil
    {
        public const string OrderNo = "OrderNo";
        public const string UserNo = "UserNo";
        public const string PlanNo = "PlanNo";
        public const string Start_Time = "_st";
        public const string End_Time = "_et";
        public const string Start_Int = "_si";
        public const string End_Int = "_ei";
        public const string End_String = "_es";
        public const string DDL_String = "_ds";
        public const string DDL_UnString = "_un";
        public const string CookieName = "KeWeiOMS_User";
        public const string BPicPath = "/ProductImg/BPic/";
        public const string SPicPath = "/ProductImg/SPic/";
        public static object obj1 = new object();
        public static object obj2 = new object();
        public static object obj3 = new object();
        public static object obj4 = new object();



        /// <summary>
        /// 将String转换为Dictionary类型，过滤掉为空的值，首先 6 分割，再 7 分割
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<string, string> StringToDictionary(string value)
        {
            Dictionary<string, string> queryDictionary = new Dictionary<string, string>();
            string[] s = value.Split('^');
            for (int i = 0; i < s.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(s[i]) && !s[i].Contains("undefined"))
                {
                    var ss = s[i].Split('&');
                    if (ss.Length == 2)
                    {
                        if ((!string.IsNullOrEmpty(ss[0])) && (!string.IsNullOrEmpty(ss[1])))
                        {
                            queryDictionary.Add(ss[0], ss[1]);
                        }
                    }
                }

            }
            return queryDictionary;
        }

        public static string XmlSerialize<T>(T obj)
        {
            string xmlString = string.Empty;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, obj);
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }
            return xmlString;
        }
        public static string Resolve(string search, bool iso = true)
        {
            string where = string.Empty;
            int flagWhere = 0;

            Dictionary<string, string> queryDic = StringToDictionary(search);
            if (queryDic != null && queryDic.Count > 0)
            {
                foreach (var item in queryDic)
                {
                    if (flagWhere != 0)
                    {
                        where += " and ";
                    }
                    flagWhere++;

                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(Start_Time)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(Start_Time)) + " >= '" + item.Value + "'";
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(End_Time)) //需要查询的列名
                    {
                        DateTime date = Convert.ToDateTime(item.Value);
                        if (date.Hour == 0 && date.Minute == 0 && iso)
                            where += item.Key.Remove(item.Key.IndexOf(End_Time)) + " <=  '" + date.ToString("yyyy-MM-dd 23:59:59") + "'";
                        else
                            where += item.Key.Remove(item.Key.IndexOf(End_Time)) + " <=  '" + item.Value + "'";
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(Start_Int)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(Start_Int)) + " >= " + item.Value;
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(End_Int)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(End_Int)) + " <= " + item.Value;
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(End_String)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(End_String)) + " = '" + item.Value + "'";
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(DDL_String)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(DDL_String)) + " = '" + item.Value + "'";
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value) && item.Key.Contains(DDL_UnString)) //需要查询的列名
                    {
                        where += item.Key.Remove(item.Key.IndexOf(DDL_String)) + " <> '" + item.Value + "'";
                        continue;
                    }
                    where += item.Key + " like '%" + item.Value + "%'";
                }
            }
            return where;
        }

        public static string GetObjEditString(object o1, object o2)
        {
            StringBuilder sb = new StringBuilder();
            System.Reflection.PropertyInfo[] properties = o1.GetType().GetProperties();

            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                if (name.StartsWith("rows"))
                {
                    continue;
                }
                object value = item.GetValue(o1, null);
                object value2 = item.GetValue(o2, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (value == null)
                        value = "";
                    if (value2 == null)
                        value2 = "";
                    if (value.ToString() != value2.ToString())
                    {
                        sb.Append(" " + name + "从“" + value + "” 修改为 “" + value2 + "”<br>");
                    }

                }
            }
            return sb.ToString();
        }

        

        public static string OrderBy(string sort, string order, string defaultSort)
        {
            string orderby = " order by " + defaultSort;
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(order))
            {
                orderby = " order by " + sort + " " + order;
            }
            return orderby;
        }
        public static string OrderBy(string sort, string order)
        {
            return OrderBy(sort, order, "Id desc");

        }
        public static string SqlWhere(string search)
        {
            //search=HttpUtility.UrlDecode(search);
            string where = string.Empty;
            if (!string.IsNullOrEmpty(search))
            {
                where = StringUtil.Resolve(search);
                if (where.Length > 0)
                {
                    where = " where " + where;
                }
            }
            return where;
        }

    }

}