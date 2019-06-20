using Microservices.Demo.Core.Database.Relational;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Microservices.Demo.Core.MVC.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TransactionAttribute : ActionFilterAttribute
    {
        public TransactionAttribute()
        {
            this.Order = 2;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IDbContext context = filterContext.HttpContext.RequestServices.GetService(typeof(IDbContext)) as IDbContext;
            if (context == null)
            {
                throw new NotImplementedException();
            }

            context.Database.BeginTransaction();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            IDbContext context = filterContext.HttpContext.RequestServices.GetService(typeof(IDbContext)) as IDbContext;
            if (context == null)
            {
                throw new NotImplementedException();
            }

            if (filterContext.Exception == null)
            {
                context.Database.CommitTransaction();
            }
            else
            {
                context.Database.RollbackTransaction();
            }
        }
    }
}
