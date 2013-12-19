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
    /// UserLoginTypeMap
    /// 用户登陆日志
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
    public class UserLoginTypeMap : BaseEntityMap<UserLoginType>
    {
        public UserLoginTypeMap()
        {
            Table("UserLogin");
            Id(x => x.Id);
            Map(x => x.UserCode).Length(255);
            Map(x => x.UserName).Length(255);
            Map(x => x.SignInOn);
            Map(x => x.IP).Length(255);
            Map(x => x.MAC).Length(255);
        }
    }
}
