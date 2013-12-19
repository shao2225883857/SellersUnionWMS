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
    /// UserRoleTypeMap
    /// 用户角色关系表
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
    public class UserRoleTypeMap : BaseEntityMap<UserRoleType> 
    {
        public UserRoleTypeMap()
        {
            Table("UserRole");
            Id(x => x.Id);
            Map(x => x.UserId);
            Map(x => x.RoleId);
        }
    }
}
