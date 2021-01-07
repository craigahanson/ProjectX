using ProjectX.Data.Scope;

namespace ProjectX.Data.EntityFrameworkCore.Scope
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly int? commandTimeout;
        private readonly string connectionString;

        public DbContextFactory(string connectionString, int? commandTimeout)
        {
            this.connectionString = connectionString;
            this.commandTimeout = commandTimeout;
        }

        public TDbContext CreateDbContext<TDbContext>() where TDbContext : class
        {
            return (TDbContext) (object) new ProjectXDbContext(connectionString, commandTimeout);
        }
    }
}