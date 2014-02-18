//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012-12-30 , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// OrderProductType
    /// 订单商品表
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
    public class OrderProductType : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public virtual int OId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public virtual String OrderNo { get; set; }

        /// <summary>
        /// 外部商品SKU
        /// </summary>
        public virtual String ExSKU { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual String Title { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Qty { get; set; }

        /// <summary>
        /// 对应内部库存SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        public virtual String Remark { get; set; }

        /// <summary>
        /// 产品规格
        /// </summary>
        public virtual String Standard { get; set; }

        /// <summary>
        /// 产品状态0：正常，1：缺货，2：停产，3：该产品已经占用了库存
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary>
        public virtual double Price { get; set; }

        /// <summary>
        /// 产品网址
        /// </summary>
        public virtual String Url { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public virtual String ImgUrl { get; set; }

    }
}
