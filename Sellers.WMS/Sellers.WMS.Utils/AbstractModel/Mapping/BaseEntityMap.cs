using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Sellers.WMS.Utils.AbstractModel
{
    public abstract class BaseEntityMap<T> : ClassMap<T> where T : BaseEntity
    {
        public BaseEntityMap()
        {
            Id(t => t.Id);
            Map(t => t.CreateOn);
            Map(t => t.CreateBy);
            Map(t => t.ModifyOn);
            Map(t => t.ModifyBy);
            DynamicInsert();
            DynamicUpdate();
        }
    }
}
