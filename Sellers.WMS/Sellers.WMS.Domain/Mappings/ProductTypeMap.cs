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
    /// ProductTypeMap
    /// 商品表
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
    public class ProductTypeMap : BaseEntityMap<ProductType> 
    {
        public ProductTypeMap()
        {
            Table("Products");
            Id(x => x.Id);
            Map(x => x.TempSKU);
            Map(x => x.SKU).Length(50);
            Map(x => x.Title).Length(300);
            Map(x => x.Status).Length(10);
            Map(x => x.ProductAttr).Length(100);
            Map(x => x.Category).Length(50);
            Map(x => x.Model).Length(50);
            Map(x => x.Brand).Length(50);
            Map(x => x.Standard).Length(200);
            Map(x => x.Price);
            Map(x => x.Weight);
            Map(x => x.Day7);
            Map(x => x.Day15);
            Map(x => x.Day30);
            Map(x => x.Long);
            Map(x => x.Wide);
            Map(x => x.High);
            Map(x => x.DayByStock);
            Map(x => x.Memo).Length(2000);
            Map(x => x.ImgPath).Length(300);
        }
    }
}
