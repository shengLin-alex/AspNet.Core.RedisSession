using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Core.RedisSession.Public.Models;
using AspNet.Core.RedisSession.Service.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Power.Mvc.Helper;

namespace AspNet.Core.RedisSession.Service
{
    public abstract class AuthServiceBase : IAuthService
    {
        private readonly IHttpContextAccessor HttpContextAccessor;

        private readonly ICacheHelper CacheHelper;

        protected string IdentKey { get; set; }
        
        protected string IdentPassword { get; set; }
        
        protected string ResultMessage { get; set; }
        
//        public virtual DateTime? ExpirationTime { get; set; }
        
        protected virtual bool IsEnableSessionIdAuth => false;

        protected virtual int SessionIdCacheLife => 24 * 60 * 60;
        
        protected AuthServiceBase(IHttpContextAccessor httpContextAccessor, ICacheHelper cacheHelper)
        {
            this.HttpContextAccessor = httpContextAccessor;
            this.CacheHelper = cacheHelper;
        }

        public virtual bool IsAuthenticated =>
            this.HttpContextAccessor.HttpContext.User?.Identity != null &&
            this.HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        
        public virtual UserInfo GetCurrentUser()
        {
            string userId = "";
            if (this.HttpContextAccessor.HttpContext.User?.Identity != null && 
                this.HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                userId = this.HttpContextAccessor.HttpContext.User.Claims.First().Value;
            }

            return new UserInfo
            {
                UserId = userId
            };
        }

        public virtual async Task<IResult<UserInfo>> Login(string identKey, string identPassword)
        {
            this.IdentKey = identKey;
            this.IdentPassword = identPassword;
            IResult<UserInfo> result = new Result<UserInfo>();
            
            if (this.Verify())
            {
                UserInfo user = new UserInfo
                {
                    UserId = this.IdentKey
                };
                await this.UserLogin(user);
                result.SetMessage(this.ResultMessage);
                result.Data = user;

                if (!this.IsEnableSessionIdAuth)
                {
                    return result;
                }

                try
                {
                    this.CacheHelper.Set($"SessionId: {this.IdentKey}", this.HttpContextAccessor.HttpContext.Session.Id, this.SessionIdCacheLife);
                }
                catch (Exception e)
                {
                    throw new AuthServiceException("Error occur while writing SessionId to cache.", e);
                }

                return result;
            }
            
            result.SetError(this.ResultMessage);

            return result;
        }

        public virtual async Task<IResult> Logout()
        {
            IResult result = new Result();
            await this.UserLogout();

            return result;
        }

        protected abstract bool Verify();

        protected async Task UserLogin(UserInfo user)
        {
            Claim[] claims = { new Claim( "UserId",user.UserId) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims , CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            
            await this.HttpContextAccessor.HttpContext.SignInAsync(principal, new AuthenticationProperties {IsPersistent = false});
        }

        protected async Task UserLogout()
        {
            await this.HttpContextAccessor.HttpContext.SignOutAsync();
        }
    }
}