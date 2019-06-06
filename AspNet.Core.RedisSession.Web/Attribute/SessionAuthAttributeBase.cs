using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Power.Mvc.Helper;
using Power.Mvc.Helper.Extensions;

namespace AspNet.Core.RedisSession.Web
{
    public abstract class SessionAuthAttributeBase : ActionFilterAttribute
    {
        private const string IsInitial = "IsInitial";
        
        protected abstract string SessionIdCacheKey { get; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ICacheHelper cacheHelper = PackageDiResolver.Current.GetService<ICacheHelper>();
            IHttpContextAccessor httpContextAccessor = PackageDiResolver.Current.GetService<IHttpContextAccessor>();
            
            if (!filterContext.HttpContext.Session.GetObject<bool>(IsInitial))
            {
                filterContext.HttpContext.Session.SetObject(IsInitial, true);
            }

            if (!filterContext.ActionDescriptor.IsDefined<AllowAnonymousFilter>() && !filterContext.ActionDescriptor.IsControllerDefined<AllowAnonymousFilter>())
            {
                string cacheSessionId = cacheHelper.Get<string>(this.SessionIdCacheKey);
                if (cacheSessionId.IsNullOrEmpty())
                {
                    this.OnCacheSessionIdMissing(filterContext);
                }
                else
                {
                    cacheSessionId = cacheSessionId.Replace(@"""", string.Empty);
                    string currentSessionId = httpContextAccessor.HttpContext.Session.Id;

                    if (!this.OnAuthorize(cacheSessionId, currentSessionId))
                    {
                        this.OnAuthorizeFail(filterContext);
                    }
                    else
                    {
                        this.OnPass(filterContext);
                    }
                }
            }
            
            base.OnActionExecuting(filterContext);
        }
        
        protected abstract void OnCacheSessionIdMissing(ActionExecutingContext filterContext);
        
        protected abstract bool OnAuthorize(string cacheSessionId, string currentSessionId);
        
        protected abstract void OnPass(ActionExecutingContext filterContext);
        
        protected abstract void OnAuthorizeFail(ActionExecutingContext filterContext);
    }
}