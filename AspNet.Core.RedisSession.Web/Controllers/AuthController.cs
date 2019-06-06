using AspNet.Core.RedisSession.Public.Models;
using AspNet.Core.RedisSession.Service.Model;
using AspNet.Core.RedisSession.Service;
using AspNet.Core.RedisSession.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Power.Mvc.Helper.Extensions;
using System.Threading.Tasks;

namespace AspNet.Core.RedisSession.Web.Controllers
{
    /// <summary>
    /// Controller for user authorize
    /// </summary>
    public class AuthController : Controller
    {
        /// <summary>
        /// Authorize service
        /// </summary>
        private readonly IAuthService AuthService;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="authService">authorize service</param>
        public AuthController(IAuthService authService)
        {
            this.AuthService = authService;
        }
        
        /// <summary>
        /// Login action that return view.
        /// </summary>
        /// <param name="returnUrl">return url when login success</param>
        /// <returns>view for login action</returns>
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

        /// <summary>
        /// Login action for post request.
        /// </summary>
        /// <param name="viewModel">login view model</param>
        /// <returns>action when login success</returns>
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
        
        /// <summary>
        /// Logout action
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await this.AuthService.Logout();

            return this.RedirectToAction("Login", "Auth");
        }
        
        /// <summary>
        /// Private action for redirect to local url, for avoid redirect attack.
        /// </summary>
        /// <param name="redirectUrl">url for redirect</param>
        /// <returns>redirect to action.</returns>
        private IActionResult RedirectToLocal(string redirectUrl)
        {
            if (Url.IsLocalUrl(redirectUrl))
            {
                return this.Redirect(redirectUrl);
            }
            
            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}