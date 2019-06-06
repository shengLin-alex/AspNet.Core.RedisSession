using AspNet.Core.RedisSession.Service;
using AspNet.Core.RedisSession.Public.Models;
using Microsoft.AspNetCore.Http;
using Power.Mvc.Helper;

namespace AspNet.Core.RedisSession.Web
{
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