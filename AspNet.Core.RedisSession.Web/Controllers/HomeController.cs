using AspNet.Core.RedisSession.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNet.Core.RedisSession.Web.Controllers
{
    /// <summary>
    /// Home page controller.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// index page
        /// </summary>
        /// <returns>index page</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// privacy page
        /// </summary>
        /// <returns>privacy page</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Error page
        /// </summary>
        /// <returns>error page</returns>
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}