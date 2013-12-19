using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sellers.WMS.Utils.Exporter
{

        public interface IDataGetter
        {
            object GetData(HttpContext context);
        }
}
