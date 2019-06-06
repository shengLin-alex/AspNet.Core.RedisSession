namespace AspNet.Core.RedisSession.Web.Models
{
    public class LoginViewModel
    {
        public string UserId { get; set; }
        
        public string UserPassword { get; set; }
        
        public string ReturnUrl { get; set; }
    }
}