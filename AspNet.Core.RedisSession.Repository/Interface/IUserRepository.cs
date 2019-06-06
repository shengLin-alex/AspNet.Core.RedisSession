using AspNet.Core.RedisSession.Repository.Model;
using Power.Repository.EFCore;

namespace AspNet.Core.RedisSession.Repository
{
    public interface IUserRepository : IRepositoryGeneric<User, PostgreSqlDbContext>
    {
    }
}