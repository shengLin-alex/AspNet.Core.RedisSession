using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace AspNet.Core.RedisSession.Web
{
    public static class ActionDescriptorExtensions
    {
        public static bool IsDefined<T>(this ActionDescriptor descriptor) where T : IFilterMetadata
        {
            bool result = descriptor.FilterDescriptors.Any(d => d.Filter.GetType() == typeof(T));

            return result;
        }
        
        public static bool IsControllerDefined<T>(this ActionDescriptor descriptor) where T : IFilterMetadata
        {
            if (descriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                return controllerActionDescriptor.FilterDescriptors.Any(d => d.Filter.GetType() == typeof(T));
            }

            return false;
        }
    }
}