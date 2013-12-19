//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// ProductSKUType
    /// 商品SKU表
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
    public class ProductSKUType : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 父编号
        /// </summary>
        public virtual String ParentSKU { get; set; }

        /// <summary>
        /// 子编号
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public virtual String Title { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public virtual double Price { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public virtual String Memo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Qty { get; set; }

        /// <summary>
        /// 库存天数
        /// </summary>
        public virtual int DayOfStock { get; set; }

    }
}
