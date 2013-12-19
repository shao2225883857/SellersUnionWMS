using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentData;

namespace Sellers.WMS.Data
{
    public class FluentDataUtil
    {

        private IDbContext Context()
        {
            return new DbContext().ConnectionStringName("MyDatabase", new SqlServerProvider());
        }

        public static bool Save<T>(T t)
        {
            using (var context = Context())
            {
                int id = context.Insert<T>("Product", t.
                        .AutoMap(x => x.Id)
                        .ExecuteReturnLastId<int>();

                product.Id = id;
                return id > 0;
            }
        }

    }
}
