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
    /// CountryTypeMap
    /// 国家表
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
    public class CountryTypeMap : BaseEntityMap<CountryType> 
    {
        public CountryTypeMap()
        {
            Table("Countrys");
            Id(x => x.Id);
            Map(x => x.CCountry).Length(50);
            Map(x => x.ECountry).Length(50);
            Map(x => x.CountryCode).Length(50);
        }
    }
}
