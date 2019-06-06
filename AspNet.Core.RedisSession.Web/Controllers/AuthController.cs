using System.Threading.Tasks;
using AspNet.Core.RedisSession.Service;
using AspNet.Core.RedisSession.Service.Model;
using AspNet.Core.RedisSession.Web.Models;
using AspNet.Core.RedisSession.Public.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Power.Mvc.Helper;
using Power.Mvc.Helper.Extensions;

namespace AspNet.Core.RedisSession.Web.Controllers
{
    [SessionAuth]
    public class AuthController : Controller
    {
        private readonly IAuthService AuthService;

        public AuthController(IAuthService authService)
        {
            this.AuthService = authService;
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            if (this.AuthService.IsAuthenticated)
            {
                return this.RedirectToLocal("/");
            }
            
            LoginViewModel viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl?.Equals(this.Url.Action(nameof(this.Logout), "Auth")) == false
                    ? returnUrl
                    : "/"
            };
            
            return this.View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            IResult<UserInfo> result = await this.AuthService.Login(viewModel.UserId, viewModel.UserPassword);
            if (!result.Success)
            {
                return this.View(viewModel);
            }

            return viewModel.ReturnUrl.IsNullOrEmpty() ? this.RedirectToAction(nameof(HomeController.Index), "Home") : this.RedirectToLocal(viewModel.ReturnUrl);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await this.AuthService.Logout();

            return this.RedirectToAction("Login", "Auth");
        }
        
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            
            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}