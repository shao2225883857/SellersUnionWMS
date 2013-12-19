using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sellers.WMS.Domain;

namespace Sellers.WMS.Web.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            List<ModuleType> list = GetAll<ModuleType>().ToList();
            list = list.OrderByDescending(f => f.SortCode).ToList();
            var items = new List<ModuleType>();
            //循环加载第一层
            foreach (var functionType in list.Where(p => p.ParentId == 0))
            {
                //从all中加载自己的子菜单
                List<ModuleType> subs = list.Where(p => p.ParentId == functionType.Id).OrderByDescending(f => f.SortCode).ToList();
                //如果有
                if (subs.Any())
                {
                    List<ModuleType> subitems = new List<ModuleType>();
                    //循环添加子菜单
                    foreach (var subtype in subs)
                    {
                        subitems.Add(subtype);
                    }
                    functionType.children = subitems;
                }
                items.Add(functionType);
            }

            return View(items);
        }

        public ActionResult Default()
        {
            return View();
        }

        public ActionResult Nav()
        {
            return View();
        }

    }
}
