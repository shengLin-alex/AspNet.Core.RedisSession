using AspNet.Core.RedisSession.Public.Models;
using AspNet.Core.RedisSession.Service;
using Microsoft.AspNetCore.Http;
using Power.Mvc.Helper;

namespace AspNet.Core.RedisSession.Web
{
    /// <summary>
    /// current user context
    /// </summary>
    public class UserContext : IUserContext<UserInfo>
    {
        private readonly IHttpContextAccessor HttpContextAccessor;

        private readonly IAuthService AuthService;

        public UserContext(IHttpContextAccessor httpContextAccessor, IAuthService authService)
        {
            this.HttpContextAccessor = httpContextAccessor;
            this.AuthService = authService;
        }

        public UserInfo Current => this.AuthService.GetCurrentUser();
    }
}