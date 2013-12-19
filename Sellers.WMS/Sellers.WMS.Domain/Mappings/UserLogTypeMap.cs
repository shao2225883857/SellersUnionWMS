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
    /// UserLogTypeMap
    /// 用户操作日志表
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
    public class UserLogTypeMap : BaseEntityMap<UserLogType> 
    {
        public UserLogTypeMap()
        {
            Table("UserLog");
            Id(x => x.Id);
            Map(x => x.UId);
            
            Map(x => x.Memo).Length(800);
        }
    }
}
