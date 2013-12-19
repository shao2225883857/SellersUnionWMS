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
    /// PermissionScopeTypeMap
    /// 操作权限域
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
    public class PermissionScopeTypeMap : BaseEntityMap<PermissionScopeType> 
    {
        public PermissionScopeTypeMap()
        {
            Table("PermissionScope");
            Id(x => x.Id);
            Map(x => x.ResourceCategory).Length(50);
            Map(x => x.ResourceId);
            Map(x => x.TargetCategory).Length(50);
            Map(x => x.TargetId);
        }
    }
}
