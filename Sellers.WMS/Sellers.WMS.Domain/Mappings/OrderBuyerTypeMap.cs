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
    /// OrderBuyerTypeMap
    /// 订单买家表
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
    public class OrderBuyerTypeMap : BaseEntityMap<OrderBuyerType> 
    {
        public OrderBuyerTypeMap()
        {
            Table("OrderBuyer");
            Id(x => x.Id);
            Map(x => x.BuyerName).Length(40);
            Map(x => x.BuyerEmail).Length(100);
            Map(x => x.BuyCount);
            Map(x => x.BuyAmount);
            Map(x => x.FristBuyOn);
            Map(x => x.LastBuyOn);
            Map(x => x.Remark).Length(400);
            Map(x => x.BuyerType).Length(40);
            Map(x => x.Platform).Length(40);
        }
    }
}
