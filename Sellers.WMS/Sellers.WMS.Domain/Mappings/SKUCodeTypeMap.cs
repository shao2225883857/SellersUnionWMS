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
    /// SKUCodeTypeMap
    /// SKUCode
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
    public class SKUCodeTypeMap : BaseEntityMap<SKUCodeType> 
    {
        public SKUCodeTypeMap()
        {
            Table("SKUCode");
            Id(x => x.Id);
            Map(x => x.Code).Length(20);
            Map(x => x.SKU).Length(20);
            Map(x => x.IsOut);
            Map(x => x.IsNew);
        }
    }
}
