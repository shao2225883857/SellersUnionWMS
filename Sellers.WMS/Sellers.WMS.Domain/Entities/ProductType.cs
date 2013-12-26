//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2013-12-23 , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// ProductType
    /// 商品表
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
    public class ProductType : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 临时编号
        /// </summary>
        public virtual String TempSKU { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public virtual String Title { get; set; }

        /// <summary>
        /// 商品状态
        /// </summary>
        public virtual String Status { get; set; }

        /// <summary>
        /// 商品特性
        /// </summary>
        public virtual String ProductAttr { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public virtual String Category { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public virtual String Model { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public virtual String Brand { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public virtual String Standard { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public virtual double Price { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public virtual int Weight { get; set; }

        /// <summary>
        /// 7Day
        /// </summary>
        public virtual double Day7 { get; set; }

        /// <summary>
        /// 15Day
        /// </summary>
        public virtual double Day15 { get; set; }

        /// <summary>
        /// 30Day
        /// </summary>
        public virtual double Day30 { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        public virtual int Long { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public virtual int Wide { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public virtual int High { get; set; }

        /// <summary>
        /// 库存天数
        /// </summary>
        public virtual int DayByStock { get; set; }

        /// <summary>
        /// 简单描述
        /// </summary>
        public virtual String Memo { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public virtual String ImgPath { get; set; }

    }
}
