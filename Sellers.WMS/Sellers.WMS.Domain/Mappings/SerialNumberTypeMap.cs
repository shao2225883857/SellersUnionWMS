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
    /// SerialNumberTypeMap
    /// 序列号表
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
    public class SerialNumberTypeMap : BaseEntityMap<SerialNumberType> 
    {
        public SerialNumberTypeMap()
        {
            Table("SerialNumber");
            Id(x => x.Id);
            Map(x => x.Code).Length(50);
            Map(x => x.BeginNo);
        }
    }
}
