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
    /// PrintTemplateTypeMap
    /// 打印模板
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
    public class PrintTemplateTypeMap : BaseEntityMap<PrintTemplateType> 
    {
        public PrintTemplateTypeMap()
        {
            Table("PrintTemplate");
            Id(x => x.Id);
            Map(x => x.Code).Length(50);
            Map(x => x.TempName).Length(50);
            Map(x => x.TempType).Length(50);
            Map(x => x.Content);
            Map(x => x.Description).Length(200);
        }
    }
}
