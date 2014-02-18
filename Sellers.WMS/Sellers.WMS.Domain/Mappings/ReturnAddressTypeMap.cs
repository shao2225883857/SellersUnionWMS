//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2014-01-06 , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;
using FluentNHibernate.Mapping;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// ReturnAddresTypeMap
    /// 回邮地址表
    /// 
    /// 修改纪录
    /// 
    ///  版本：1.0 Dean 创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name>Dean</name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class ReturnAddresTypeMap : BaseEntityMap<ReturnAddressType> 
    {
        public ReturnAddresTypeMap()
        {
            Table("ReturnAddress");
            Id(x => x.Id);
            Map(x => x.RetuanName).Length(50);
            Map(x => x.Phone).Length(50);
            Map(x => x.Tel).Length(50);
            Map(x => x.PostCode).Length(50);
            Map(x => x.Street).Length(500);
        }
    }
}
