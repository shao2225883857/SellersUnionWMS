//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2013-12-23 , Dean TECH, Ltd.
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
    ///  版本：1.0 Dean 创建主键。
    /// 
    /// 版本：1.0
    /// 
    /// <author>
    /// <name>Dean</name>
    /// <date></date>
    /// </author>
    /// </summary>
    public class ProductPriceRecrodType : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 主SKU
        /// </summary>
        public virtual String MainSKU { get; set; }

        /// <summary>
        /// 子SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 采购批次
        /// </summary>
        public virtual String PurchaseBatch { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public virtual double Price { get; set; }

        /// <summary>
        /// 和上一次相差
        /// </summary>
        public virtual double Difference { get; set; }

    }
}
