﻿//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// ProductCategoryType
    /// 类目
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
    public class ProductCategoryType : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public virtual int ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int SortCode { get; set; }

    }
}
