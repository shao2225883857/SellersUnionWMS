//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// DataDictionaryType
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
    public class DataDictionaryType : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public virtual String ClassName { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public virtual String Code { get; set; }

        /// <summary>
        /// 系统内置
        /// </summary>
        public virtual int AllowDelete { get; set; }

    }
}
