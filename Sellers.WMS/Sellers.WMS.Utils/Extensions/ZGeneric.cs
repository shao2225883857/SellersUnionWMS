using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sellers.WMS.Utils.Extensions
{
    public class ZGeneric
    {
        public static bool IsTypeIgoreNullable<T>(T t)
        {
            if (t == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
