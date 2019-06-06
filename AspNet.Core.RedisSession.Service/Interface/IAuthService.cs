using System.Threading.Tasks;
using AspNet.Core.RedisSession.Public.Models;
using AspNet.Core.RedisSession.Service.Model;

namespace AspNet.Core.RedisSession.Service
{
    /// <summary>
    /// Authorize service interface
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Get website authenticated for browser
        /// </summary>
        bool IsAuthenticated { get; }
        
        /// <summary>
        /// Get current user
        /// </summary>
        /// <returns>current user information</returns>
        UserInfo GetCurrentUser();

        /// <summary>
        /// Login task
        /// </summary>
        /// <param name="identKey">user identity key</param>
        /// <param name="identPassword">user identity password</param>
        /// <returns>Login request result</returns>
        Task<IResult<UserInfo>> Login(string identKey, string identPassword);

        /// <summary>
        /// Logout task
        /// </summary>
        /// <returns>Logout request result</returns>
        Task<IResult> Logout();
    }
}