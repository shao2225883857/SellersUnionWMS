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
    /// DataDictionaryTypeMap
    /// 数据字典
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
    public class DataDictionaryTypeMap : BaseEntityMap<DataDictionaryType> 
    {
        public DataDictionaryTypeMap()
        {
            Table("DataDictionary");
            Id(x => x.Id);
            Map(x => x.ClassName).Length(200);
            Map(x => x.Code).Length(200);
            Map(x => x.AllowDelete);
        }
    }
}
