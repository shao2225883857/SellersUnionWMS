using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sellers.WMS.Utils.Extensions
{
    public static class ZConvert
    {
        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }


        public static int ToInt(string str)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public static int ToInt(object obj)
        {
            try
            {
                if (obj == null || obj is DBNull)
                {
                    return 0;
                }
                return ToInt(obj.ToString());
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public static double ToDouble(object str)
        {
            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception)
            {
                return 0;
            }

        }


        public static string ToStr(object obj)
        {
            try
            {
                if (obj is DBNull || obj == null)
                    return "";
                return obj.ToString();
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
