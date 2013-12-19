using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sellers.WMS.Web
{
    public class MenuItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 父节点主键
        /// </summary>
        public virtual int ParentId { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public virtual String Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String FullName { get; set; }

        /// <summary>
        /// 图标编号
        /// </summary>
        public virtual String ImageIndex { get; set; }

        /// <summary>
        /// 导航地址
        /// </summary>
        public virtual String NavigateUrl { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        public virtual int SortCode { get; set; }

        /// <summary>
        /// 二级菜单
        /// </summary>
        public virtual List<MenuItem> childrens { get; set; }


        public static List<MenuItem> GetMenu()
        {


            
            var list = new List<MenuItem>();
            list.Add(new MenuItem { Id = 1, ParentId = 0, Code = "YW", FullName = "订单管理", ImageIndex = "", NavigateUrl = "", SortCode = 100 });
            list.Add(new MenuItem { Id = 2, ParentId = 0, Code = "Base", FullName = "基础设置", ImageIndex = "", NavigateUrl = "", SortCode = 90 });
            list.Add(new MenuItem { Id = 3, ParentId = 1, Code = "Order", FullName = "订单列表", ImageIndex = "", NavigateUrl = "/AliOrder/Index", SortCode = 90 });
            list.Add(new MenuItem { Id = 4, ParentId = 2, Code = "Country", FullName = "菜单管理", ImageIndex = "", NavigateUrl = "/Module/Index", SortCode = 90 });
            list.Add(new MenuItem { Id = 5, ParentId = 2, Code = "Address", FullName = "发货地址", ImageIndex = "", NavigateUrl = "/AliShop/AddressEdit", SortCode = 90 });
            list.Add(new MenuItem { Id = 6, ParentId = 2, Code = "Template", FullName = "模板设置", ImageIndex = "", NavigateUrl = "/PrintTemplate/Index", SortCode = 90 });
            list.Add(new MenuItem { Id = 7, ParentId = 2, Code = "Shop", FullName = "店铺管理", ImageIndex = "", NavigateUrl = "/AliShop/Index", SortCode = 90 });
            list.Add(new MenuItem { Id = 8, ParentId = 2, Code = "Shop", FullName = "给客户的留言", ImageIndex = "", NavigateUrl = "/AliShop/SellerMemoEdit", SortCode = 90 });
            list.Add(new MenuItem { Id = 9, ParentId = 1, Code = "Shop", FullName = "缺货订单", ImageIndex = "", NavigateUrl = "/AliOrder/QueList", SortCode = 90 });
            list.Add(new MenuItem { Id = 10, ParentId = 1, Code = "Shop", FullName = "发货扫描", ImageIndex = "", NavigateUrl = "/AliOrder/OrderScanBySend", SortCode = 90 });
            list.Add(new MenuItem { Id = 11, ParentId = 1, Code = "Shop", FullName = "缺货扫描", ImageIndex = "", NavigateUrl = "/AliOrder/OrderScanByQue", SortCode = 90 });



            var items = new List<MenuItem>();

            //循环加载第一层
            foreach (var functionType in list.Where(p => p.ParentId == 0))
            {
                //从all中加载自己的子菜单
                List<MenuItem> subs = list.Where(p => p.ParentId == functionType.Id).OrderByDescending(f => f.SortCode).ToList();
                //如果有
                if (subs.Any())
                {
                    List<MenuItem> subitems = new List<MenuItem>();
                    //循环添加子菜单
                    foreach (var subtype in subs)
                    {
                        subitems.Add(subtype);
                    }
                    functionType.childrens = subitems;
                }
                items.Add(functionType);
            }

            return items;
        }
    }
}