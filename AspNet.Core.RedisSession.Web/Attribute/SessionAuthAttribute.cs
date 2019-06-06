using AspNet.Core.RedisSession.Web.Controllers;
using AspNet.Core.RedisSession.Public.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Power.Mvc.Helper;
using Power.Mvc.Helper.Extensions;

namespace AspNet.Core.RedisSession.Web
{
    public class SessionAuthAttribute : SessionAuthAttributeBase
    {
        protected override string SessionIdCacheKey
        {
            get
            {
                IUserContext<UserInfo> user = PackageDiResolver.Current.GetService<IUserContext<UserInfo>>();
                string key = $"SessionId: { user.Current.UserId }";

                return key;
            }
        }
        
        protected override bool OnAuthorize(string cacheSessionId, string currentSessionId)
        {
            return cacheSessionId == currentSessionId;
        }
        
        protected override void OnAuthorizeFail(ActionExecutingContext filterContext)
        {
            filterContext.RedirectToActionResult(nameof(AuthController.Logout), "Auth");
        }
        
        protected override void OnCacheSessionIdMissing(ActionExecutingContext filterContext)
        {
            filterContext.RedirectToActionResult(nameof(AuthController.Logout), "Auth");
        }
        
        protected override void OnPass(ActionExecutingContext filterContext)
        {
        }
    }
}