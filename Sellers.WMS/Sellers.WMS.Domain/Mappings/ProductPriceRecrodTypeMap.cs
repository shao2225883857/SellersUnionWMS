﻿//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;
using FluentNHibernate.Mapping;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// ProductPriceRecrodTypeMap
    /// 商品价格历史
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0  创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name></name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class ProductPriceRecrodTypeMap : BaseEntityMap<ProductPriceRecrodType> 
    {
        public ProductPriceRecrodTypeMap()
        {
            Table("ProductPriceRecrod");
            Id(x => x.Id);
            Map(x => x.SKU);
            Map(x => x.OldSKU);
            Map(x => x.N);
        }
    }
}
