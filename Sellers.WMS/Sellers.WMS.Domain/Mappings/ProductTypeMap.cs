//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , Dean TECH, Ltd.
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
    ///  版本：1.0  创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name></name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class ProductTypeMap : BaseEntityMap<ProductType> 
    {
        public ProductTypeMap()
        {
            Table("Products");
            Id(x => x.Id);
            Map(x => x.SKU).Length(50);
            Map(x => x.Title).Length(200);
            Map(x => x.Status).Length(10);
            Map(x => x.Category).Length(50);
            Map(x => x.Model).Length(50);
            Map(x => x.Brand).Length(50);
            Map(x => x.Standard).Length(100);
            Map(x => x.Price);
            Map(x => x.Weight);
            Map(x => x.Long);
            Map(x => x.Wide);
            Map(x => x.High);
            Map(x => x.DayByStock);
            Map(x => x.Summary).Length(2000);
            Map(x => x.PackMemo).Length(200);
            Map(x => x.IsInfraction);
            Map(x => x.PicUrl).Length(200);
            Map(x => x.SPicUrl).Length(200);
            Map(x => x.Purchaser).Length(50);
            Map(x => x.Examiner).Length(50);
            Map(x => x.Packer).Length(50);
            Map(x => x.PackCoefficient);
            Map(x => x.IsElectronic);
            Map(x => x.HasBattery);
            Map(x => x.Location).Length(50);
           
        }
    }
}
