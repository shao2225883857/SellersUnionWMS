//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// CurrencyType
    /// 汇率表
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
    public class CurrencyType : BaseEntity
    {
        /// <summary>
        /// 主键表
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public virtual String CurrencyName { get; set; }

        /// <summary>
        /// 符号
        /// </summary>
        public virtual String CurrencySign { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public virtual Decimal CurrencyValue { get; set; }

        /// <summary>
        /// 是否自动更新
        /// </summary>
        public virtual int IsAutoUpdate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdateOn { get; set; }

    }
}
