using AspNet.Core.RedisSession.Repository;
using AspNet.Core.RedisSession.Repository.Model;
using Microsoft.AspNetCore.Http;
using Power.Mvc.Helper;

namespace AspNet.Core.RedisSession.Service
{
    public class AuthService : AuthServiceBase
    {
        private readonly IUserRepository UserRepository;

        protected override bool IsEnableSessionIdAuth => true;

        public AuthService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, ICacheHelper cacheHelper) : base(httpContextAccessor, cacheHelper)
        {
            this.UserRepository = userRepository;
        }

        protected override bool Verify()
        {
            User user = this.UserRepository.Get(u => u.Id == this.IdentKey);
            
            return user != null && user.Password == this.IdentPassword;
        }
    }
}