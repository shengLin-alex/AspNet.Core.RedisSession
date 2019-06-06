using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNet.Core.RedisSession.Web
{
    public static class ActionExecutionContextExtensions
    {
        public static void RedirectToActionResult(this ActionExecutingContext context, string actionName, string controllerName)
        {
            Controller controller = (Controller)context.Controller;
            context.Result = controller.RedirectToAction(actionName, controllerName);
        }
    }
}