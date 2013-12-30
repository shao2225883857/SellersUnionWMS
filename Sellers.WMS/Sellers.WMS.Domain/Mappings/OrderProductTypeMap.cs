//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012-12-30 , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;
using FluentNHibernate.Mapping;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// OrderProductTypeMap
    /// 订单商品表
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0 Dean 创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name>Dean</name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class OrderProductTypeMap : BaseEntityMap<OrderProductType> 
    {
        public OrderProductTypeMap()
        {
            Table("OrderProducts");
            Id(x => x.Id);
            Map(x => x.OId);
            Map(x => x.OrderNo).Length(200);
            Map(x => x.ExSKU).Length(40);
            Map(x => x.Title).Length(400);
            Map(x => x.Qty);
            Map(x => x.SKU).Length(40);
            Map(x => x.Remark).Length(400);
            Map(x => x.Standard).Length(400);
            Map(x => x.Price);
            Map(x => x.Url).Length(200);
        }
    }
}
