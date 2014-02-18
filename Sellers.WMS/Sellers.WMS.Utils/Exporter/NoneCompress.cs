using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sellers.WMS.Utils.Exporter
{
    public class NoneCompress : ICompress
    {
        string ICompress.Suffix(string orgSuffix)
        {
            throw new NotImplementedException();
        }

        System.IO.Stream ICompress.Compress(System.IO.Stream fileStream, string fullName)
        {
            throw new NotImplementedException();
        }
    }
}
