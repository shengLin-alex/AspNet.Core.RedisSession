using Power.Mvc.Helper;

namespace AspNet.Core.RedisSession.Public.Models
{
    public class UserInfo : IUserInfo
    {
        public string UserId { get; set; }
        
        public string UserName { get; set; }
    }
}