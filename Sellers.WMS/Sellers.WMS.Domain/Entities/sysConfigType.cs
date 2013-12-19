//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// sysConfigType
    /// sysConfig
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
    public class sysConfigType : BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public virtual String Key { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public virtual String Value { get; set; }

    }
}
