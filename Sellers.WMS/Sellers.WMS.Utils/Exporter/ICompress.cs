using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sellers.WMS.Utils.Exporter
{
    public interface ICompress
    {
        string Suffix(string orgSuffix);
        Stream Compress(Stream fileStream, string fullName);
    }
}
