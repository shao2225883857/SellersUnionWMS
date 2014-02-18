using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using Sellers.WMS.Data.Repository;
using Sellers.WMS.Domain;
using Sellers.WMS.Utils.AbstractModel;
using Sellers.WMS.Utils.Exceptions;
using Sellers.WMS.Web.Controllers.Filters;

namespace Sellers.WMS.Web.Controllers
{
    [Authorize(Order = 0)]
    [AllowAnonymous]
    public class BaseController : System.Web.Mvc.Controller
    {
        public BaseController()
        {

        }

        protected bool permissionAdd = false;
        protected bool permissionEdit = false;
        protected bool permissionDelete = false;
        protected bool permissionExport = false;

        private UserType currentUser;

        public UserType CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    currentUser = GetCurrentAccount();
                }
                return currentUser;
            }
        }

        /// <summary>
        /// 获取当前登陆人的账户信息
        /// </summary>
        /// <returns>账户信息</returns>
        private UserType GetCurrentAccount()
        {

            if (Session["user"] != null)
            {
                UserType user = (UserType)Session["user"];
                return user;
            }
            return null;
            //return new UserType { Id = 0, Realname = "邵锡栋" };
        }

        public virtual void GetPermission()
        {


        }

        public bool IsAuthorized(string code)
        {
            return true;
        }

   
        public virtual string BuildToolBarButtons(int t)
        {
            return "";
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            List<string> ExFilterField = new List<string>();
            ExFilterField.Add("LOGIN");
            ExFilterField.Add("REG");
            ExFilterField.Add("LOGOFF");
            ExFilterField.Add("PRINTSAVE");
            bool iscon = false;
            var controller = filterContext.RouteData.Values["controller"].ToString().ToString();
            var action = filterContext.RouteData.Values["action"].ToString().ToUpper();
            if (ExFilterField.Contains(action))
            {
                return;
            }
            if (filterContext.HttpContext.Session["user"] == null)
            {
                string str = Common.GetCookies();
                if (!string.IsNullOrEmpty(str))
                {
                    if (str.IndexOf('&') != -1)
                    {
                        string[] strs = str.Split('&');
                        iscon = Common.LoginByUser(strs[0], strs[1], NSession);
                        //IsAuthorization(filterContext, controller, action);
                    }
                }
                if (!iscon)
                {
                    filterContext.HttpContext.Response.Write(" <script type='text/javascript'> window.top.location='/User/Login/'; </script>");
                    filterContext.Result = new EmptyResult();
                    return;
                }
            }
        }



        /// <summary>
        /// ISession
        /// </summary>
        public ISession NSession { get { return NhbHelper.GetCurrentSession(); } }



        //保存数据
        public virtual bool Save<T>(T entity)
        {
            try
            {
                NSession.Save(entity);
                if (!NSession.Transaction.IsActive)// 不是在事务里，就刷新到数据库
                    NSession.Flush();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("保存数据失败...", ex);

            }
            return true;
        }

        //更新数据
        public virtual bool Update<T>(T entity)
        {
            try
            {
                NSession.Update(entity);
                if (!NSession.Transaction.IsActive)// 不是在事务里，就刷新到数据库
                    NSession.Flush();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("更新数据失败...", ex);
            }
            return true;
        }

        //保存或更新数据
        public virtual bool SaveOrUpdate<T>(T entity)
        {
            try
            {
                NSession.SaveOrUpdate(entity);
                if (!NSession.Transaction.IsActive)// 不是在事务里，就刷新到数据库
                    NSession.Flush();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("保存或更新数据失败...", ex);
            }
            return true;
        }

        //物理删除数据
        public virtual bool DeleteObj<T>(T entity)
        {
            try
            {
                NSession.Delete(entity);
                if (!NSession.Transaction.IsActive)// 不是在事务里，就刷新到数据库
                    NSession.Flush();
            }
            catch (System.Exception ex)
            {
                throw new RepositoryException("删除数据失败...", ex);
            }
            return true;
        }

        //物理删除数据
        public virtual bool DeleteObj<T>(object id)
        {
            try
            {
                var entity = Get<T>(id);
                NSession.Delete(entity);
                if (!NSession.Transaction.IsActive)// 不是在事务里，就刷新到数据库
                    NSession.Flush();
            }
            catch (System.Exception ex)
            {
                throw new RepositoryException("删除数据失败...", ex);
            }
            return true;
        }

        //获取数据（如果为空抛异常）
        public T Get<T>(object id)
        {
            try
            {
                T entity = NSession.Get<T>(id);
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
        public T Load<T>(string id)
        {
            try
            {
                T entity = NSession.Load<T>(id);
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

        //判断字段的值是否存在 如果是插入id赋值-1或者new Guid,如果是修改id赋值 要修改项的值
        public bool IsFieldExist<T>(string fieldName, string fieldValue, string id)
        {
            return IsFieldExist<T>(fieldName, fieldValue, id, null);
        }

        //判断字段的值是否在一定范围内已存在 如果是插入id赋值-1或者new Guid,如果是修改id赋值 要修改项的值
        public bool IsFieldExist<T>(string fieldName, string fieldValue, string id, string where)
        {
            if (!string.IsNullOrEmpty(where))
                where = @" and " + where;
            var query = NSession.CreateQuery(
                string.Format(@"select count(*) from {0} as o where o.{1}='{2}' and o.Id<>'{3}'" + where,
                typeof(T).Name,
                fieldName,
                fieldValue, id));
            return query.UniqueResult<long>() > 0;
        }

        public int GetListCount<T>(string where)
        {
            if (!string.IsNullOrEmpty(where))
                where = @" where " + where;
            var query = NSession.CreateQuery(
               string.Format(@"select count(*) from {0} " + where,
               typeof(T).Name
               ));
            return query.UniqueResult<int>();
        }

        //简单查询
        public List<T> GetList<T>(string fieldName, string fieldValue, string where)
        {
            if (!string.IsNullOrEmpty(where))
                where = @" and " + where;
            var query = NSession.CreateQuery(
                string.Format(@"from {0} as o where o.{1}='{2}' " + where,
                typeof(T).Name,
                fieldName,
                fieldValue));
            return query.List<T>().ToList<T>();
        }

        //获取所有列表
        public virtual List<T> GetAll<T>() where T : BaseEntity
        {
            return NSession.CreateCriteria<T>()
                        .List<T>().ToList();
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
