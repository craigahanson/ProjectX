using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectX.Data.EntityFrameworkCore
{
    public class ProjectXDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProjectXDbContext>
    {
        public ProjectXDbContext CreateDbContext(string[] args)
        {
            return new ProjectXDbContext("Server=(local);Database=ProjectX;User ID=sa;Password=Welcome123;Trusted_Connection=False;", null);
        }
    }
}
