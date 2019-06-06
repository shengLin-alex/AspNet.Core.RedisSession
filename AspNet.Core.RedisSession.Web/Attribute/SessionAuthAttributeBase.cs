using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Power.Mvc.Helper.Extensions;
using Power.Mvc.Helper;

namespace AspNet.Core.RedisSession.Web
{
    /// <summary>
    /// Session authorize filter attribute base class.
    /// This filter is for avoiding duplicate login from different browser.
    /// </summary>
    public abstract class SessionAuthAttributeBase : ActionFilterAttribute
    {
        /// <summary>
        /// Session key for set if is initial.
        /// </summary>
        private const string IsInitial = "IsInitial";
        
        /// <summary>
        /// Key for get Session Id in cache
        /// </summary>
        protected abstract string SessionIdCacheKey { get; }

        /// <summary>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        /// <param name="filterContext">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // get service needed.
            ICacheHelper cacheHelper = PackageDiResolver.Current.GetService<ICacheHelper>();
            IHttpContextAccessor httpContextAccessor = PackageDiResolver.Current.GetService<IHttpContextAccessor>();
            
            // check if session is initialized.
            if (!filterContext.HttpContext.Session.GetObject<bool>(IsInitial))
            {
                filterContext.HttpContext.Session.SetObject(IsInitial, true);
            }

            // check if action decorated by AllowAnonymousFilter
            if (!filterContext.ActionDescriptor.IsDefined<AllowAnonymousFilter>() && !filterContext.ActionDescriptor.IsControllerDefined<AllowAnonymousFilter>())
            {
                // get session id in cache
                string cacheSessionId = cacheHelper.Get<string>(this.SessionIdCacheKey);
                if (cacheSessionId.IsNullOrEmpty())
                {
                    this.OnCacheSessionIdMissing(filterContext);
                }
                else
                {
                    cacheSessionId = cacheSessionId.Replace(@"""", string.Empty);
                    string currentSessionId = httpContextAccessor.HttpContext.Session.Id;

                    // compare session in cache with current session id. if different, then fail.
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
        
        /// <summary>
        /// act when session id not in cache.
        /// </summary>
        /// <param name="filterContext">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
        protected abstract void OnCacheSessionIdMissing(ActionExecutingContext filterContext);
        
        /// <summary>
        /// implement for authorize
        /// </summary>
        /// <param name="cacheSessionId">session id in cache</param>
        /// <param name="currentSessionId">session id for current session</param>
        /// <returns>pass or not</returns>
        protected abstract bool OnAuthorize(string cacheSessionId, string currentSessionId);
        
        /// <summary>
        /// act when pass
        /// </summary>
        /// <param name="filterContext">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
        protected abstract void OnPass(ActionExecutingContext filterContext);
        
        /// <summary>
        /// act when authorize fail.
        /// </summary>
        /// <param name="filterContext">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
        protected abstract void OnAuthorizeFail(ActionExecutingContext filterContext);
    }
}