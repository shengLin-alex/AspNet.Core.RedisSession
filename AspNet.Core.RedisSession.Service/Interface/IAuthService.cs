using System.Threading.Tasks;
using AspNet.Core.RedisSession.Public.Models;
using AspNet.Core.RedisSession.Service.Model;

namespace AspNet.Core.RedisSession.Service
{
    public interface IAuthService
    {
        bool IsAuthenticated { get; }
        
        UserInfo GetCurrentUser();

        Task<IResult<UserInfo>> Login(string identKey, string identPassword);

        Task<IResult> Logout();
    }
}