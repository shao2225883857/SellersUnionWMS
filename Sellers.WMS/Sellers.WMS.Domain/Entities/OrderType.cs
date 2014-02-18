//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012-12-30 , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// OrderType
    /// 订单表
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
    public class OrderType : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public virtual String OrderNo { get; set; }

        /// <summary>
        /// 外部编号
        /// </summary>
        public virtual String OrderExNo { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public virtual string Status { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public virtual int Enabled { get; set; }

        /// <summary>
        /// 是否打印
        /// </summary>
        public virtual int IsPrint { get; set; }

        /// <summary>
        /// 合并订单
        /// </summary>
        public virtual int IsMerger { get; set; }

        /// <summary>
        /// 拆分订单
        /// </summary>
        public virtual int IsSplit { get; set; }

        /// <summary>
        /// 缺货订单
        /// </summary>
        public virtual int IsOutOfStock { get; set; }

        /// <summary>
        /// 重发订单
        /// </summary>
        public virtual int IsRepeat { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public virtual String CurrencyCode { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public virtual double Amount { get; set; }

        /// <summary>
        /// 流水交易号
        /// </summary>
        public virtual String TId { get; set; }

        /// <summary>
        /// 买家
        /// </summary>
        public virtual String BuyerName { get; set; }

        /// <summary>
        /// 买家邮箱
        /// </summary>
        public virtual String BuyerEmail { get; set; }

        /// <summary>
        /// 买家Id
        /// </summary>
        public virtual int BuyerId { get; set; }

        /// <summary>
        /// 买家留言
        /// </summary>
        public virtual String BuyerMemo { get; set; }

        /// <summary>
        /// 商家留言
        /// </summary>
        public virtual String SellerMemo { get; set; }

        /// <summary>
        /// 发货方式
        /// </summary>
        public virtual String LogisticMode { get; set; }

        /// <summary>
        /// 发货方式
        /// </summary>
        public virtual String TrackCode { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public virtual String Country { get; set; }

        /// <summary>
        /// 地址Id
        /// </summary>
        public virtual int AddressId { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public virtual int Weight { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public virtual DateTime GenerateOn { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public virtual DateTime ScanOn { get; set; }
        /// <summary>
        /// 生成时间
        /// </summary>
        public virtual string ScanBy { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public virtual String Account { get; set; }

        /// <summary>
        /// ValiInfo
        /// </summary>
        public virtual String ValiInfo { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public virtual String Platform { get; set; }

        public virtual List<OrderProductType> ProductList { get; set; }

        public virtual OrderAddressType Address { get; set; }

    }
}
