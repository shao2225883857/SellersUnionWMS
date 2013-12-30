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
    /// OrderTypeMap
    /// 订单表
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
    public class OrderTypeMap : BaseEntityMap<OrderType> 
    {
        public OrderTypeMap()
        {
            Table("Orders");
            Id(x => x.Id);
            Map(x => x.OrderNo).Length(40);
            Map(x => x.OrderExNo).Length(200);
            Map(x => x.Status);
            Map(x => x.IsPrint);
            Map(x => x.IsMerger);
            Map(x => x.IsSplit);
            Map(x => x.IsOutOfStock);
            Map(x => x.IsRepeat);
            Map(x => x.CurrencyCode).Length(10);
            Map(x => x.Amount);
            Map(x => x.TId).Length(200);
            Map(x => x.BuyerName).Length(40);
            Map(x => x.BuyerEmail).Length(80);
            Map(x => x.BuyerId);
            Map(x => x.BuyerMemo).Length(1000);
            Map(x => x.SellerMemo).Length(1000);
            Map(x => x.LogisticMode).Length(40);
            Map(x => x.Country).Length(40);
            Map(x => x.AddressId);
            Map(x => x.Weight);
            Map(x => x.GenerateOn);
            Map(x => x.Account).Length(40);
            Map(x => x.Platform).Length(40);
        }
    }
}
