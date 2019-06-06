using Power.Repository.EFCore;

namespace AspNet.Core.RedisSession.Repository
{
    /// <summary>
    /// PostgreSqlDbContext factory
    /// </summary>
    public class PostgreSqlDbContextFactory : DbContextFactoryBase<PostgreSqlDbContext>, IPostgreSqlDbContextFactory<PostgreSqlDbContext>
    {
        /// <summary>
        /// Key for this dbContext ConnectionString
        /// </summary>
        protected override string ConnectionKey { get; set; } = "PostgreSqlLocal";
    }
}