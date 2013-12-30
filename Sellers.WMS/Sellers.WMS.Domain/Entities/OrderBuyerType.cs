//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012-12-30 , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// OrderBuyerType
    /// 订单买家表
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
    public class OrderBuyerType : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual String BuyerName { get; set; }

        /// <summary>
        /// 客户邮件
        /// </summary>
        public virtual String BuyerEmail { get; set; }

        /// <summary>
        /// 客户购买次数
        /// </summary>
        public virtual int BuyCount { get; set; }

        /// <summary>
        /// 客户购买金额
        /// </summary>
        public virtual double BuyAmount { get; set; }

        /// <summary>
        /// 第一次购买时间
        /// </summary>
        public virtual DateTime FristBuyOn { get; set; }

        /// <summary>
        /// 最后一次购买时间
        /// </summary>
        public virtual DateTime LastBuyOn { get; set; }

        /// <summary>
        /// 客户备注
        /// </summary>
        public virtual String Remark { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public virtual String BuyerType { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public virtual String Platform { get; set; }

    }
}
