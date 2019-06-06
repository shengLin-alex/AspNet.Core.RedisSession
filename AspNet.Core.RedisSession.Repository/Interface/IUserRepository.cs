using AspNet.Core.RedisSession.Repository.Model;
using Power.Repository.EFCore;

namespace AspNet.Core.RedisSession.Repository
{
    /// <summary>
    /// User data repository interface
    /// </summary>
    public interface IUserRepository : IRepositoryGeneric<User, PostgreSqlDbContext>
    {
    }
}