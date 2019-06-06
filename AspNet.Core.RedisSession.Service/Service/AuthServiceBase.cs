using AspNet.Core.RedisSession.Public.Models;
using AspNet.Core.RedisSession.Service.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Power.Mvc.Helper;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace AspNet.Core.RedisSession.Service
{
    /// <summary>
    /// Authorize service base class
    /// </summary>
    public abstract class AuthServiceBase : IAuthService
    {
        /// <summary>
        /// Http context accessor
        /// </summary>
        private readonly IHttpContextAccessor HttpContextAccessor;

        /// <summary>
        /// Cache helper
        /// </summary>
        private readonly ICacheHelper CacheHelper;

        /// <summary>
        /// User identity key
        /// </summary>
        protected string IdentKey { get; set; }
        
        /// <summary>
        /// User identity key
        /// </summary>
        protected string IdentPassword { get; set; }
        
        /// <summary>
        /// Request result
        /// </summary>
        protected string ResultMessage { get; set; }
        
//        public virtual DateTime? ExpirationTime { get; set; }
        
        /// <summary>
        /// Set true if enable avoid duplicate login from different browser
        /// </summary>
        protected virtual bool IsEnableSessionIdAuth => false;

        /// <summary>
        /// Session cache lifetime
        /// </summary>
        protected virtual int SessionIdCacheLife => 24 * 60 * 60;
        
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="httpContextAccessor">http context accessor</param>
        /// <param name="cacheHelper">cache helper</param>
        protected AuthServiceBase(IHttpContextAccessor httpContextAccessor, ICacheHelper cacheHelper)
        {
            this.HttpContextAccessor = httpContextAccessor;
            this.CacheHelper = cacheHelper;
        }

        /// <summary>
        /// Get website authenticated for browser
        /// </summary>
        public virtual bool IsAuthenticated =>
            this.HttpContextAccessor.HttpContext.User?.Identity != null &&
            this.HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        
        /// <summary>
        /// Get current user
        /// </summary>
        /// <returns>current user information</returns>
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
        
        /// <summary>
        /// Login task
        /// </summary>
        /// <param name="identKey">user identity key</param>
        /// <param name="identPassword">user identity password</param>
        /// <returns>Login request result</returns>
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

        /// <summary>
        /// Logout task
        /// </summary>
        /// <returns>Logout request result</returns>
        public virtual async Task<IResult> Logout()
        {
            IResult result = new Result();
            await this.UserLogout();

            return result;
        }

        /// <summary>
        /// Verify if user allow to login this website.
        /// </summary>
        /// <returns>pass or not</returns>
        protected abstract bool Verify();

        /// <summary>
        /// Cookie based authentication login.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Task for sign in user</returns>
        protected async Task UserLogin(UserInfo user)
        {
            Claim[] claims = { new Claim( "UserId",user.UserId) };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims , CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            
            await this.HttpContextAccessor.HttpContext.SignInAsync(principal, new AuthenticationProperties {IsPersistent = false});
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns>Task for sign out user</returns>
        protected async Task UserLogout()
        {
            await this.HttpContextAccessor.HttpContext.SignOutAsync();
        }
    }
}