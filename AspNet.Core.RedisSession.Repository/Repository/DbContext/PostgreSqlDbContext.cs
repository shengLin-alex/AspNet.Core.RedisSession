using AspNet.Core.RedisSession.Repository.Model;
using Microsoft.EntityFrameworkCore;
using Power.Repository.EFCore;

namespace AspNet.Core.RedisSession.Repository
{
    /// <summary>
    /// PostgreSqlDbContext
    /// </summary>
    public class PostgreSqlDbContext : PostgreSqlDbContextBase
    {
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="connectionString">連線字串</param>
        public PostgreSqlDbContext(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// TestEntity 資料集
        /// </summary>
        public DbSet<User> UserEntity { get; set; }

        /// <summary>
        ///     Override this method to further configure the model that was discovered by convention from the entity types
        ///     exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        ///     and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <remarks>
        ///     If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        ///     then this method will not be run.
        /// </remarks>
        /// <param name="modelBuilder">
        ///     The builder being used to construct the model for this context. Databases (and other extensions) typically
        ///     define extension methods on this object that allow you to configure aspects of the model that are specific
        ///     to a given database.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure default schema
            modelBuilder.HasDefaultSchema("dotnetcore");
            base.OnModelCreating(modelBuilder);
        }
    }
}