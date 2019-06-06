using Power.Mvc.Helper;

namespace AspNet.Core.RedisSession.Public.Models
{
    /// <summary>
    /// User information for this website.
    /// </summary>
    public class UserInfo : IUserInfo
    {
        /// <summary>
        /// user identity
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// user name
        /// </summary>
        public string UserName { get; set; }
    }
}