using System.Runtime.InteropServices.ComTypes;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectX.Data.EntityFrameworkCore
{
    public class ProjectXDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProjectXDbContext>
    {
        public ProjectXDbContext CreateDbContext(string[] args)
        {
            return new ProjectXDbContext(args[0], int.TryParse(args[1], out int timeout) ? timeout : null);
        }
    }
}