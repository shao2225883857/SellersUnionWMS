using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sellers.WMS.Utils.AbstractModel
{
    /// <summary>
    /// 录入数据类型基类
    /// </summary>
    public abstract class InputItem : BaseEntity
    {
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 显示值
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        public virtual string PY { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>
        public virtual string WB { get; set; }

        /// <summary>
        /// 助记码
        /// </summary>
        public virtual string InputCode { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int IndexField { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }
    }
}
