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
    /// RoleTypeMap
    /// 系统角色表
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
    public class RoleTypeMap : BaseEntityMap<RoleType> 
    {
        public RoleTypeMap()
        {
            Table("Roles");
            Id(x => x.Id);
            Map(x => x.Code).Length(50);
            Map(x => x.Realname).Length(200);
            Map(x => x.SortCode);
            Map(x => x.Description).Length(200);
        }
    }
}
