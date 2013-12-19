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
    /// ProductIsInfractionTypeMap
    /// 商品侵权表
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
    public class ProductIsInfractionTypeMap : BaseEntityMap<ProductIsInfractionType> 
    {
        public ProductIsInfractionTypeMap()
        {
            Table("ProductIsInfraction");
            Id(x => x.Id);
            Map(x => x.OldSKU).Length(255);
            Map(x => x.SKU).Length(255);
            Map(x => x.Platform).Length(255);
            Map(x => x.Isinfraction);
        }
    }
}
