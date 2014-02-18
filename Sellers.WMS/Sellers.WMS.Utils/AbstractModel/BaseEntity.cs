using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sellers.WMS.Utils.AbstractModel
{
    /// <summary>
    /// 数据实体基类
    /// </summary>
    public abstract class BaseEntity
    {
        public BaseEntity()
        {

            this.CreateOn = DateTime.Now;
            this.ModifyOn = DateTime.Now;
        }
        /// <summary>
        /// 主键
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 数据创建时间
        /// </summary>
        public virtual DateTime CreateOn { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual String CreateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime ModifyOn { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public virtual String ModifyBy { get; set; }


        public virtual void SetId(int id)
        {
            this.Id = id;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Entity return false.
            BaseEntity entity = obj as BaseEntity;
            if ((System.Object)entity == null)
            {
                return false;
            }

            if (Id == 0)
            {
                return false;
            }
            //if (Id == null || Id == string.Empty || entity.Id == null || entity.Id == string.Empty)
            //{
            //    return false;
            //}

            // Return true if the Id match:
            return Id == entity.Id;
        }

        public static bool operator ==(BaseEntity a, BaseEntity b)
        {
            // If both are null or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            if (a.Id == 0)
                return false;
            //if (Id == null || Id == string.Empty || entity.Id == null || entity.Id == string.Empty)
            //{
            //    return false;
            //}

            // Return true if the Id match:
            return a.Id == b.Id;
        }

        public override int GetHashCode()
        {
            return Id == 0 ? base.GetHashCode() : Id.GetHashCode();
        }

        public static bool operator !=(BaseEntity a, BaseEntity b)
        {
            return !(a == b);
        }
    }
}
