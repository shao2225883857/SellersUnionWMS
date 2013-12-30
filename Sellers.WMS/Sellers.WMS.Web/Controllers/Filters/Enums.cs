using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sellers.WMS.Web.Controllers
{
    public enum ResourceCategoryEnum
    {
        User,
        Role,
        Department
    }
    public enum TargetCategoryEnum
    {
        Module,
        PermissionItem,
        Account
    }

    public enum OrderStatusEnum
    {
        待处理 = 0,
        已处理 = 1,
        待拣货 = 2,
        待包装 = 3,
        待发货 = 4,
        已发货 = 5,
        已完成 = 6,
        作废订单 = 7,
        退货订单 = 9
    }
}