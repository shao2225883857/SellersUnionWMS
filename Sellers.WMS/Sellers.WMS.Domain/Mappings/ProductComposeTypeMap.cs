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
    /// ProductComposeTypeMap
    /// 组合产品标记表
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
    public class ProductComposeTypeMap : BaseEntityMap<ProductComposeType> 
    {
        public ProductComposeTypeMap()
        {
            Table("ProductCompose");
            Id(x => x.Id);
            Map(x => x.PId);
            Map(x => x.SKU).Length(50);
            Map(x => x.SrcPId);
            Map(x => x.SrcSKU).Length(50);
            Map(x => x.SrcQty);
        }
    }
}
