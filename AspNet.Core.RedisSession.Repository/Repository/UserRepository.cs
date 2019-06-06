using AspNet.Core.RedisSession.Repository.Model;
using Power.Repository.EFCore;

namespace AspNet.Core.RedisSession.Repository
{
    public class UserRepository : RepositoryGeneric<User, PostgreSqlDbContext>, IUserRepository
    {
        public UserRepository(IPostgreSqlDbContextFactory<PostgreSqlDbContext> factory) : base(factory)
        {
        }
    }
}