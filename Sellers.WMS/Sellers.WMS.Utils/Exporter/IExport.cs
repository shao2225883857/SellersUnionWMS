using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sellers.WMS.Utils.Exporter
{
    public interface IExport
    {
        string suffix { get; }

        void MergeCell(int x1, int y1, int x2, int y2);
        void FillData(int x, int y, string field, object data);

        void Init(object data);
        Stream SaveAsStream();

        void SetHeadStyle(int x1, int y1, int x2, int y2);
        void SetRowsStyle(int x1, int y1, int x2, int y2);
    }
}
