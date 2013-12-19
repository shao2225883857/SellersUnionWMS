using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Sellers.WMS.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ExceptionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //List<string> ExFilterField = new List<string>();
            //ExFilterField.Add("LOGIN");
            //ExFilterField.Add("REG");
            //ExFilterField.Add("LOGOFF");
            //bool iscon = false;
            //var controller = filterContext.RouteData.Values["controller"].ToString().ToString();
            //var action = filterContext.RouteData.Values["action"].ToString().ToUpper();
            //if (ExFilterField.Contains(action))
            //{
            //    return;
            //}
            //if (filterContext.HttpContext.Session["aliUser"] == null)
            //{
            //    filterContext.HttpContext.Response.Write(" <script type='text/javascript'> window.top.location='/AliUserUser/Login/'; </script>");
            //    filterContext.Result = new EmptyResult();
            //    return;
            //}
        }
    }
}
