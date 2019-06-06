using Autofac;
using Power.Mvc.Helper.Extensions;
using Power.Mvc.Helper;
using System.Reflection;

namespace AspNet.Core.RedisSession.Repository
{
    /// <summary>
    /// Type register
    /// </summary>
    public class AutofacTypeRegister : ITypeRegister
    {
        /// <summary>
        /// register order
        /// </summary>
        public int Seq => 3;

        /// <summary>
        /// register types
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        public void RegisterTypes(ContainerBuilder builder)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            builder
                .RegisterAssemblyTypes(assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .LogExpectRegistration();
        }
    }
}