using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

using System.Linq.Expressions;
using Sellers.WMS.Utils.AbstractModel;
using Sellers.WMS.Utils.Exceptions;


namespace Sellers.WMS.Data.Repository
{
    /// <summary>
    /// 数据库访问基类
    /// </summary>
    public class GeneralRepository<T> where T : BaseEntity
    {
        public ISession Session { get { return NhbHelper.GetCurrentSession(); } }

        public virtual IQueryOver<T, T> QueryOver()
        {

            return Session.QueryOver<T>();
        }
        public virtual IQueryOver<T, T> QueryOver(Expression<Func<T>> alias)
        {
            return Session.QueryOver<T>(alias);
        }

        //获取所有列表
        public virtual IList<T> GetAll()
        {
            return Session.CreateCriteria<T>()
                        .List<T>();
        }

        //保存数据
        public virtual void Save(T entity)
        {
            try
            {
                Session.Save(entity);
                if (!Session.Transaction.IsActive)// 不是在事务里，就刷新到数据库
                    Session.Flush();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("保存数据失败...", ex);
            }
        }

        //更新数据
        public virtual void Update(T entity)
        {
            try
            {
                Session.Update(entity);
                if (!Session.Transaction.IsActive)// 不是在事务里，就刷新到数据库
                    Session.Flush();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("更新数据失败...", ex);
            }
        }

        //保存或更新数据
        public virtual void SaveOrUpdate(T entity)
        {
            try
            {
                Session.SaveOrUpdate(entity);
                if (!Session.Transaction.IsActive)// 不是在事务里，就刷新到数据库
                    Session.Flush();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("保存或更新数据失败...", ex);
            }
        }

        //物理删除数据
        public virtual void Delete(T entity)
        {
            try
            {
                Session.Delete(entity);
                if (!Session.Transaction.IsActive)// 不是在事务里，就刷新到数据库
                    Session.Flush();
            }
            catch (System.Exception ex)
            {
                throw new RepositoryException("删除数据失败...", ex);
            }
        }
        //物理删除数据
        public virtual void Delete(string id)
        {
            try
            {
                var entity = Get(id);
                Session.Delete(entity);
                if (!Session.Transaction.IsActive)// 不是在事务里，就刷新到数据库
                    Session.Flush();
            }
            catch (System.Exception ex)
            {
                throw new RepositoryException("删除数据失败...", ex);
            }
        }
        //获取数据（如果为空抛异常）
        public T Get(string id)
        {
            try
            {
                T entity = Session.Get<T>(id);
                if (entity == null)
                    throw new NullException("返回数据为空...");
                else
                    return entity;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("获取数据失败...", ex);
            }
        }

        //获取数据 ，不会访问数据库（如果为空抛异常）
        public T Load(string id)
        {
            try
            {
                T entity = Session.Load<T>(id);
                if (entity == null)
                    throw new NullException("返回数据为空...");
                else
                    return entity;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("获取数据失败...", ex);
            }
        }

        //判断字段的值是否在一定范围内已存在 如果是插入id赋值-1或者new Guid,如果是修改id赋值 要修改项的值
        public bool IsFieldExist(string fieldName, string fieldValue, string id, string where)
        {
            if (!string.IsNullOrEmpty(where))
                where = @" and " + where;
            var query = Session.CreateQuery(
                string.Format(@"select count(*) from {0} as o where o.{1}='{2}' and o.Id<>'{3}'" + where,
                typeof(T).Name,
                fieldName,
                fieldValue, id));
            return query.UniqueResult<long>() > 0;
        }

        //判断字段的值是否存在 如果是插入id赋值-1或者new Guid,如果是修改id赋值 要修改项的值
        public bool IsFieldExist(string fieldName, string fieldValue, string id)
        {
            return IsFieldExist(fieldName, fieldValue, id, null);
        }

        private NHibernate.Criterion.ICriterion GetComparison(string comparison, string field, object value)
        {
            NHibernate.Criterion.ICriterion res;
            switch (comparison)
            {
                case "lt":
                    res = NHibernate.Criterion.Restrictions.Lt(field, value);
                    break;
                case "gt":
                    res = NHibernate.Criterion.Restrictions.Gt(field, value);
                    break;
                case "eq":
                    res = NHibernate.Criterion.Restrictions.Eq(field, value);
                    break;
                case "elt":
                    res = NHibernate.Criterion.Restrictions.Le(field, value);
                    break;
                case "egt":
                    res = NHibernate.Criterion.Restrictions.Ge(field, value);
                    break;
                default:
                    res = null;
                    break;
            }
            return res;
        }

    }
}
