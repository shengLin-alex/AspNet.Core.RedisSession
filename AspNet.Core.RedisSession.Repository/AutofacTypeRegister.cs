using System.Reflection;
using Autofac;
using Power.Mvc.Helper;
using Power.Mvc.Helper.Extensions;

namespace AspNet.Core.RedisSession.Repository
{
    /// <summary>
    /// 類別註冊
    /// </summary>
    public class AutofacTypeRegister : ITypeRegister
    {
        /// <summary>
        /// 註冊順序
        /// </summary>
        public int Seq => 3;

        /// <summary>
        /// 註冊類別
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