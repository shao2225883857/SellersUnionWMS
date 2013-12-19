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
    /// CurrencyTypeMap
    /// 汇率表
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
    public class CurrencyTypeMap : BaseEntityMap<CurrencyType> 
    {
        public CurrencyTypeMap()
        {
            Table("Currencys");
            Id(x => x.Id);
            Map(x => x.CurrencyName).Length(30);
            Map(x => x.CurrencySign).Length(30);
            Map(x => x.CurrencyValue).Length(18);
            Map(x => x.IsAutoUpdate);
            Map(x => x.UpdateOn);
        }
    }
}
