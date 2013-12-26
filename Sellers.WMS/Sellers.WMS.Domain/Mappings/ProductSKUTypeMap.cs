//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2013-12-23 , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;
using FluentNHibernate.Mapping;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// ProductSKUTypeMap
    /// 商品SKU表
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
    public class ProductSKUTypeMap : BaseEntityMap<ProductSKUType> 
    {
        public ProductSKUTypeMap()
        {
            Table("ProductSKU");
            Id(x => x.Id);
            Map(x => x.ParentSKU).Length(50);
            Map(x => x.SKU).Length(50);
            Map(x => x.Title).Length(300);
            Map(x => x.Price);
            Map(x => x.Memo).Length(200);
            Map(x => x.Qty);
            Map(x => x.DayOfStock);
        }
    }
}
