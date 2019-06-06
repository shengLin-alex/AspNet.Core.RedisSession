using AspNet.Core.RedisSession.Repository;
using Microsoft.AspNetCore.Mvc;
using Power.Mvc.Helper;
using Power.Mvc.Helper.Extensions;

namespace AspNetCore.Core.RedisSession.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly IUserRepository UserRepo;

        private readonly ICacheHelper CacheHelper;

        public TestController(IUserRepository userRepo, ICacheHelper cacheHelper)
        {
            this.UserRepo = userRepo;
            this.CacheHelper = cacheHelper;
        }

        public IActionResult GetUsers(string id)
        {
            return this.Json(this.UserRepo.Get(u => u.Id == id));
        }

        public IActionResult SetCache(string id)
        {
            return this.Json(this.CacheHelper.Get("TestCache", () => id));
        }
    }
}