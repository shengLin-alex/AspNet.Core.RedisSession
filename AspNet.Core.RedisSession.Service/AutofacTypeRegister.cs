using System.Reflection;
using Autofac;
using Power.Mvc.Helper;
using Power.Mvc.Helper.Extensions;

namespace AspNet.Core.RedisSession.Service
{
    public class AutofacTypeRegister : ITypeRegister
    {
        public int Seq => 5;
        
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