//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2014-01-06 , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// ReturnAddressType
    /// 回邮地址表
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
    public class ReturnAddressType : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 发件人
        /// </summary>
        public virtual String RetuanName { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public virtual String Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual String Tel { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public virtual String PostCode { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public virtual String Street { get; set; }

    }
}
