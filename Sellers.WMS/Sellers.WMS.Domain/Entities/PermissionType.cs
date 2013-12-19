//--------------------------------------------------------------------
// All Rights Reserved , Copyright (C)  , Dean TECH, Ltd.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sellers.WMS.Utils.AbstractModel;

namespace Sellers.WMS.Domain
{

    /// <summary>
    /// PermissionType
    /// 操作权限存储表
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
    public class PermissionType : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 资料类别
        /// </summary>
        public virtual String ResourceCategory { get; set; }

        /// <summary>
        /// 资源主键
        /// </summary>
        public virtual String ResourceId { get; set; }

        /// <summary>
        /// 权限主键
        /// </summary>
        public virtual int PermissionId { get; set; }

    }
}
