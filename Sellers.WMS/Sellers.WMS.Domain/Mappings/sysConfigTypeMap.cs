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
    /// sysConfigTypeMap
    /// sysConfig
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
    public class sysConfigTypeMap : BaseEntityMap<sysConfigType> 
    {
        public sysConfigTypeMap()
        {
            Table("sysConfig");
            Id(x => x.Id);
            Map(x => x.Key).Length(100);
            Map(x => x.Value).Length(200);
        }
    }
}
