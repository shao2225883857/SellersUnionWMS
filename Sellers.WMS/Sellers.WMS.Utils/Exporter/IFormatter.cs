using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sellers.WMS.Utils.Exporter
{
    public interface IFormatter
    {
        object Format(object value);
    }
}
