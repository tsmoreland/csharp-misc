using IdentityDomain;
using IdentityDomain.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var migrationAssembly = GetType().Assembly.GetName().Name;

            // SQLite connection strings:
            // - default: Data Source=<filename>;Cache=Shared
            // - encrypted: Data Source=<filename>;Password=<encryption key>
            // - Read-only: Data Source=<filename>;Mode=ReadOnly
            // - In-memory: Data Source=:memory:
            // - Shared in-memory: Data Source=Sharable;Mode=Memory;Cache=Shared
            // reference: https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/connection-strings
            const string connectionString = "Data Source=identityDemo.db;Cache=Shared";
            services
                .AddDbContext<DemoDbContext>(options => 
                    options.UseSqlite(
                        connectionString, 
                        sqlOptions => sqlOptions.MigrationsAssembly(migrationAssembly)));

            services.AddIdentity<User, IdentityRole>(options => { })
                .AddEntityFrameworkStores<DemoDbContext>();

            services
                .AddAuthentication("cookies")
                .AddCookie("cookies", options => options.LoginPath = "/Home/Login");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
