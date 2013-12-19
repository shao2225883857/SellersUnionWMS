using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sellers.WMS.Utils.Exceptions
{
    /// <summary>
    /// 值为空异常
    /// </summary>
    public class NullException : ValidationException
    {
        public NullException() { }

        public NullException(string message)
            : base(message) { }

        public NullException(string message, Exception innerException)
            : base(message, innerException) { }

        public NullException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
