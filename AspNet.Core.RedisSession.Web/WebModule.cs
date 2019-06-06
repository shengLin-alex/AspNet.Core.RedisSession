using System;
using AspNet.Core.RedisSession.Public.Models;
using Autofac;
using Power.Mvc.Helper;
using Power.Mvc.Helper.Extensions;

namespace AspNet.Core.RedisSession.Web
{
    /// <summary>
    /// Base class for user-defined modules
    /// </summary>
    public class WebModule : Module
    {
        /// <summary>
        /// SettingHelper
        /// </summary>
        public string SettingHelper { get; set; }
        
        public string CacheHelper { get; set; }
        
        public string UserContext { get; set; }

        /// <summary>
        /// Add registrations to the container
        /// </summary>
        /// <param name="builder">Initializes a new instance of the Autofac.ContainerBuilder class.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType(Type.GetType(this.SettingHelper))
                .As<ISettingHelper>()
                .InstancePerLifetimeScope()
                .LogExpectRegistration();

            builder
                .RegisterType(Type.GetType(this.CacheHelper))
                .As<ICacheHelper>()
                .InstancePerLifetimeScope()
                .LogExpectRegistration();
            
            builder
                .RegisterType(Type.GetType(this.UserContext))
                .As<IUserContext<UserInfo>>()
                .InstancePerLifetimeScope()
                .LogExpectRegistration();
        }
    }
}