//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2013-12-23 , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// ProductImgType
    /// 商品图片表
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
    public class ProductImgType : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public virtual String SKU { get; set; }

        /// <summary>
        /// 主SKU
        /// </summary>
        public virtual String MainSKU { get; set; }

        /// <summary>
        /// 图片名称
        /// </summary>
        public virtual String ImgName { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public virtual String Src { get; set; }

        /// <summary>
        /// 文件保存
        /// </summary>
        public virtual Byte[] Img { get; set; }

    }
}
