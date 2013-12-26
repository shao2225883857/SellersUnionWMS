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
    /// ProductPriceRecrodTypeMap
    /// 商品价格历史
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
    public class ProductPriceRecrodTypeMap : BaseEntityMap<ProductPriceRecrodType> 
    {
        public ProductPriceRecrodTypeMap()
        {
            Table("ProductPriceRecrod");
            Id(x => x.Id);
            Map(x => x.MainSKU).Length(100);
            Map(x => x.SKU).Length(100);
            Map(x => x.PurchaseBatch).Length(200);
            Map(x => x.Price);
            Map(x => x.Difference);
        }
    }
}
