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
    /// OrderAddresTypeMap
    /// 订单买家地址表
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
    public class OrderAddressTypeMap : BaseEntityMap<OrderAddressType> 
    {
        public OrderAddressTypeMap()
        {
            Table("OrderAddress");
            Id(x => x.Id);
            Map(x => x.BId);
            Map(x => x.Addressee).Length(200);
            Map(x => x.Tel).Length(100);
            Map(x => x.Phone).Length(100);
            Map(x => x.Street).Length(400);
            Map(x => x.County).Length(100);
            Map(x => x.City).Length(100);
            Map(x => x.Province).Length(100);
            Map(x => x.Country).Length(100);
            Map(x => x.CountryCode).Length(100);
            Map(x => x.Email).Length(100);
            Map(x => x.PostCode).Length(100);
        }
    }
}
