// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectX.Library;

namespace ProjectX.Authentication
{
    public class Startup
    {
        public IConfiguration Configuration;
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //var appSettings = Configuration.CreateAppSettings();
            
            services.AddIdentityServer(options =>
                {
                    // https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/
                    options.EmitStaticAudienceClaim = true;
                })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddTestUsers(Config.Users);
                //.AddConfigurationStore(options =>
                //{
                //    options.ConfigureDbContext = builder =>
                //        builder.UseSqlServer("Server=(local);Database=ProjectX;Trusted_Connection=True;",
                //        builder.UseSqlServer(appSettings.Database.ConnectionString,
                //            sql => sql.MigrationsAssembly(migrationsAssembly));
                //})
                //.AddOperationalStore(options =>
                //{
                //    options.ConfigureDbContext = builder =>
                //        builder.UseSqlServer("Server=(local);Database=ProjectX;Trusted_Connection=True;",
                //        builder.UseSqlServer(appSettings.Database.ConnectionString,
                //            sql => sql.MigrationsAssembly(migrationsAssembly));
                //});
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseIdentityServer();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
