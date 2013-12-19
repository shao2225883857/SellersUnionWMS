using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sellers.WMS.Utils.Exceptions
{
    /// <summary>
    /// 本系统异常都要继承的基类
    /// </summary>
    [Serializable]
    public class OITException : Exception
    {
        public OITException() { }

        public OITException(string message)
            : base(message) { }

        public OITException(string message, Exception innerException)
            : base(message, innerException) { }

        public OITException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
