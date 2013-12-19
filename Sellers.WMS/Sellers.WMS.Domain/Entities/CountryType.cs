﻿//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// CountryType
    /// 国家表
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
    public class CountryType : BaseEntity
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 国家中文
        /// </summary>
        public virtual String CCountry { get; set; }

        /// <summary>
        /// 国家英文
        /// </summary>
        public virtual String ECountry { get; set; }

        /// <summary>
        /// 国家代码
        /// </summary>
        public virtual String CountryCode { get; set; }

    }
}
