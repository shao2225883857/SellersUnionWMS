//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// ProductPriceRecrodType
    /// 商品价格历史
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
    public class ProductPriceRecrodType : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual String Id { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// OldSKU
        /// </summary>
        public virtual String OldSKU { get; set; }

        /// <summary>
        /// N
        /// </summary>
        public virtual String N { get; set; }

    }
}
