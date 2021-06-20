using System;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AspNet.Core.RedisSession.Repository;
using AspNet.Core.RedisSession.Repository.Model;
using AspNet.Core.RedisSession.Service;
using Autofac;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Power.Mvc.Helper;
using Power.UnitTest.Infra;

namespace AspNet.Core.RedisSession.UnitTest
{
    [TestClass]
    public class AuthServiceTest : IoCSupportedTestBase<ContainerFactory>
    {
        [TestInitialize]
        public void TestInit()
        {
            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(r => r.Get(It.IsAny<Expression<Func<User, bool>>>())).Returns(new User
            {
                Id = "unit_test",
                Name = "unit_test_name",
                Password = "unit_test_password"
            });

            var mockCacheHelper = new Mock<ICacheHelper>();
            mockCacheHelper.Setup(r => r.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            
            var mockSession = new Mock<ISession>();
            mockSession.Setup(r => r.Id).Returns("unit_test_session_id");
            
            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(r => r.IsAuthenticated).Returns(false);
            
            var mockPrincipal = new Mock<ClaimsPrincipal>();
            mockPrincipal.Setup(r => r.Identity).Returns(mockIdentity.Object);

            var mockAuthService = new Mock<IAuthenticationService>();
            mockAuthService
                .Setup(r => r.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(r => r.GetService(typeof(IAuthenticationService)))
                .Returns(mockAuthService.Object);
            
            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(r => r.User).Returns(mockPrincipal.Object);
            mockContext.Setup(r => r.Session).Returns(mockSession.Object);
            mockContext.Setup(r => r.RequestServices).Returns(mockServiceProvider.Object);
            
            var mockAccessor = new Mock<IHttpContextAccessor>();
            mockAccessor.Setup(r => r.HttpContext).Returns(mockContext.Object);

            var authService = new AuthService(mockUserRepo.Object, mockAccessor.Object, mockCacheHelper.Object);
            
            UseExternalRegistrar(builder =>
            {
                builder.RegisterInstance(authService)
                    .As<IAuthService>()
                    .SingleInstance();
            });
        }
        
        /// <summary>
        /// 清理測試方法
        /// </summary>
        [TestCleanup]
        public void TestMethodCleanUp()
        {
            FinishUsingContainer();
        }
        
        [TestMethod]
        public void TestLoginSuccess()
        {
            IAuthService authService = this.Resolve<IAuthService>();
            var loginTask = authService.Login("unit_test", "unit_test_password");
            
            Assert.IsTrue(loginTask.Result.Success);
            Assert.IsTrue(loginTask.Result.Data.UserId.Equals("unit_test"));
        }

        [TestMethod]
        public void TestLoginFail()
        {
            IAuthService authService = this.Resolve<IAuthService>();
            var loginTask = authService.Login("unit_test", "unit_test_wrong_password");
            
            Assert.IsFalse(loginTask.Result.Success);
        }
    }
}