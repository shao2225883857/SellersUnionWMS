using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sellers.WMS.Utils.Extensions
{
    public class EachHelper
    {
        public static void EachListHeader(object data, Action<object, string, object> action)
        {

            if (data is IList<Object>)
            {
                IList<Object> list = data as List<Object>;
                Type t = list[0].GetType();
                PropertyInfo[] os = t.GetProperties();
                for (int i = 0; i < os.Count(); i++)
                {
                    action(i, os[i].Name, t);
                }
            }
        }

        public static void EachListRow(object data, Func<object, object, int> p1)
        {
            if (data is IList<Object>)
            {
                IList<Object> list = data as List<Object>;
                Type t = list[0].GetType();
                PropertyInfo[] os = t.GetProperties();
                
            }
        }

        public static void EachObjectProperty(object rowData, Action<object, string, object> action)
        {
            throw new NotImplementedException();
        }
    }
}
