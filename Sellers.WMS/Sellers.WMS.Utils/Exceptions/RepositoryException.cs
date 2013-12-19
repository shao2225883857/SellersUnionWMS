using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sellers.WMS.Utils.Exceptions
{
    /// <summary>
    /// 存储访问异常类
    /// </summary>
    public class RepositoryException : OITException
    {
        public RepositoryException() { }

        public RepositoryException(string message)
            : base(message) { }

        public RepositoryException(string message, Exception innerException)
            : base(message, innerException) { }

        public RepositoryException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
