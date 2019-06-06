using AspNet.Core.RedisSession.Repository.Model;
using AspNet.Core.RedisSession.Repository;
using Microsoft.AspNetCore.Http;
using Power.Mvc.Helper;

namespace AspNet.Core.RedisSession.Service
{
    /// <summary>
    /// Authorize service implementation
    /// </summary>
    public class AuthService : AuthServiceBase
    {
        /// <summary>
        /// User data repository
        /// </summary>
        private readonly IUserRepository UserRepository;

        /// <summary>
        /// Set enable avoid duplicate login from different browser
        /// </summary>
        protected override bool IsEnableSessionIdAuth => true;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="userRepository">user data repository</param>
        /// <param name="httpContextAccessor">http context accessor</param>
        /// <param name="cacheHelper">cache helper</param>
        public AuthService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, ICacheHelper cacheHelper) : base(httpContextAccessor, cacheHelper)
        {
            this.UserRepository = userRepository;
        }

        /// <summary>
        /// Verify if user allow to login this website.
        /// </summary>
        /// <returns>pass or not</returns>
        protected override bool Verify()
        {
            User user = this.UserRepository.Get(u => u.Id == this.IdentKey);
            
            return user != null && user.Password == this.IdentPassword;
        }
    }
}