using AspNet.Core.RedisSession.Public.Models;
using AspNet.Core.RedisSession.Web.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Power.Mvc.Helper.Extensions;
using Power.Mvc.Helper;

namespace AspNet.Core.RedisSession.Web
{
    /// <summary>
    /// Session authorize filter attribute implement.
    /// </summary>
    public class SessionAuthAttribute : SessionAuthAttributeBase
    {
        /// <summary>
        /// Key for get Session Id in cache
        /// </summary>
        protected override string SessionIdCacheKey
        {
            get
            {
                IUserContext<UserInfo> user = PackageDiResolver.Current.GetService<IUserContext<UserInfo>>();
                string key = $"SessionId: { user.Current.UserId }";

                return key;
            }
        }
        
        /// <summary>
        /// implement for authorize
        /// </summary>
        /// <param name="cacheSessionId">session id in cache</param>
        /// <param name="currentSessionId">session id for current session</param>
        /// <returns>pass or not</returns>
        protected override bool OnAuthorize(string cacheSessionId, string currentSessionId)
        {
            return cacheSessionId == currentSessionId;
        }
        
        /// <summary>
        /// act when authorize fail.
        /// </summary>
        /// <param name="filterContext">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
        protected override void OnAuthorizeFail(ActionExecutingContext filterContext)
        {
            filterContext.RedirectToActionResult(nameof(AuthController.Logout), "Auth");
        }
        
        /// <summary>
        /// act when session id not in cache.
        /// </summary>
        /// <param name="filterContext">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
        protected override void OnCacheSessionIdMissing(ActionExecutingContext filterContext)
        {
            filterContext.RedirectToActionResult(nameof(AuthController.Logout), "Auth");
        }
        
        /// <summary>
        /// act when pass
        /// </summary>
        /// <param name="filterContext">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
        protected override void OnPass(ActionExecutingContext filterContext)
        {
        }
    }
}