using System.Web;
using System.Web.Mvc;
using Sellers.WMS.Web.Controllers.Filters;

namespace Sellers.WMS.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ErrorAttribute());
        }
    }
}