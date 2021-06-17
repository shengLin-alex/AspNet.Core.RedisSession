using AspNet.Core.RedisSession.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Power.Mvc.Helper.Extensions;
using Power.Mvc.Helper;

namespace AspNet.Core.RedisSession.Web.Controllers
{
    /// <summary>
    /// Test controller for test api
    /// </summary>
    [AllowAnonymous] // not work here, cause UseEndpoint
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IUserRepository UserRepo;

        private readonly ICacheHelper CacheHelper;

        public TestController(IUserRepository userRepo, ICacheHelper cacheHelper)
        {
            this.UserRepo = userRepo;
            this.CacheHelper = cacheHelper;
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetUsers(string id)
        {
            return this.Json(this.UserRepo.Get(u => u.Id == id));
        }

        [HttpGet("[action]/{id}")]
        public IActionResult SetCache(string id)
        {
            return this.Json(this.CacheHelper.Get("TestCache", () => id));
        }
    }
}