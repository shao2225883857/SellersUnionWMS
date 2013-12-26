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
    /// ProductCategoryTypeMap
    /// 类目
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
    public class ProductCategoryTypeMap : BaseEntityMap<ProductCategoryType> 
    {
        public ProductCategoryTypeMap()
        {
            Table("ProductCategory");
            Id(x => x.Id);
            Map(x => x.ParentId);
            Map(x => x.Name).Length(50);
            Map(x => x.SortCode);
            Map(x => x.Code).Length(50); ;
        }
    }
}
