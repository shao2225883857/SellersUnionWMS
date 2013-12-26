using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sellers.WMS.Utils.Extensions
{
    public class ResultInfo
    {
        public virtual string Key { get; set; }
        public virtual string Field1 { get; set; }
        public virtual string Field2 { get; set; }
        public virtual string Field3 { get; set; }
        public virtual string Field4 { get; set; }
        /// <summary>
        /// 情况
        /// </summary>
        public virtual string Info { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public virtual string Result { get; set; }


        public virtual DateTime CreateOn { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Memo { get; set; }
    }
}
