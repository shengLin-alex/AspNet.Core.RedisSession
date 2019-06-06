using AspNet.Core.RedisSession.Repository.Model;
using Power.Repository.EFCore;

namespace AspNet.Core.RedisSession.Repository
{
    /// <summary>
    /// User data repository
    /// </summary>
    public class UserRepository : RepositoryGeneric<User, PostgreSqlDbContext>, IUserRepository
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="factory">PostgresSqlDbContext factory</param>
        public UserRepository(IPostgreSqlDbContextFactory<PostgreSqlDbContext> factory) : base(factory)
        {
        }
    }
}