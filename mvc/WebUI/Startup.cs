//
// Copyright © 2020 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using IdentityDomain;
using IdentityDomain.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

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
            services
                .AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                });

            var migrationAssembly = GetType().Assembly.GetName().Name;

            // SQLite connection strings:
            // - default: Data Source=<filename>;Cache=Shared
            // - encrypted: Data Source=<filename>;Password=<encryption key>
            // - Read-only: Data Source=<filename>;Mode=ReadOnly
            // - In-memory: Data Source=:memory:
            // - Shared in-memory: Data Source=Sharable;Mode=Memory;Cache=Shared
            // reference: https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/connection-strings
            services
                .AddDbContext<DemoDbContext>(options =>
                {
#if DEBUGGING_MIGRATIONS
                    options.EnableSensitiveDataLogging();
#endif
#if USE_SQL_LITE
                    var connectionString = Configuration.GetConnectionString("IdentityDemoSQLite");
                    options.UseSqlite(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationAssembly));
#else
                    var connectionString = Configuration.GetConnectionString("IdentityDemoSQLServer");
                    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationAssembly));
#endif
                });
            services.AddScoped<IDemoRepository, DemoRepository>();

            // look up how to handle data protection keys, for example an api spread across multiple nodes
            // each node would need to use the same key
            services.AddIdentity<DemoUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = true; // default, just stateing it explicitly
                    options.Password.RequireUppercase = true; // default, just stateing it explicitly
                    options.Password.RequireNonAlphanumeric = true; // default, just stateing it explicitly
                    options.Password.RequireDigit = true; // default, just stateing it explicitly

                    options.SignIn.RequireConfirmedEmail = true; // default, just stateing it explicitly

                    options.User.RequireUniqueEmail = true;

                    options.Tokens.EmailConfirmationTokenProvider = "demoEmailProvider";

                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);

                })
                .AddEntityFrameworkStores<DemoDbContext>()
                .AddDefaultTokenProviders() // for things like forgot password tokens
                .AddTokenProvider<EmailConfirmationTokenProvider<DemoUser>>("demoEmailProvider")
                .AddPasswordValidator<DemoPasswordValidator<DemoUser>>();
            services.AddScoped<IUserClaimsPrincipalFactory<DemoUser>, DemoUserClaimsPrincipalFactory>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Home/Login";
                options.LogoutPath = "/Home/Logout";
                options.AccessDeniedPath = "/Home/AccessDenied"; 
                //options.SessionStore = ... if we wanted to use session id, then we'd need to implement a store, see https://github.com/aspnet/Security/blob/22d2fe99c6fd9806b36025399a217a3a8b4e50f4/samples/CookieSessionSample/MemoryCacheTicketStore.cs
            });
            services.ConfigureExternalCookie(options => 
            { 
                options.LoginPath = "/Home/Login";
                options.LogoutPath = "/Home/Logout";
                options.AccessDeniedPath = "/Home/AccessDenied"; 
            });

            // intented for password reset, time is arbitrary (as in I just chose a random one without much consideration for usability)
            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromMinutes(30)); 
            services.Configure<EmailConfirmationTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromDays(2));
            services.Configure<PasswordHasherOptions>(options => 
            { 
                options.IterationCount = 100_000;
                options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3; // default made explicit
            });

            var authBuilder = services
                .AddAuthentication();
            var clientId = Configuration["Google:ClientId"];
            var clientSecret = Configuration["Google:ClientSecret"];
            if (!string.IsNullOrEmpty(clientSecret) && !string.IsNullOrEmpty(clientId))
                authBuilder.AddGoogle("google", options =>
                {
                    options.ClientId = clientId;
                    options.ClientSecret = clientSecret;
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                });

            services.AddAntiforgery(options => options.SuppressXFrameOptionsHeader = true);

            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc(config =>
            {
                config.CacheProfiles.Add("Default", new CacheProfile()
                {
                    Duration = 30,
                    Location = ResponseCacheLocation.Any
                });
                config.CacheProfiles.Add("None", new CacheProfile()
                {
                    Location = ResponseCacheLocation.None,
                    NoStore = true
                });
            });
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {"image/jpeg"});
            });
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddRazorPages();            

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
            app.UseResponseCompression();
            app.UseStaticFiles();

            // UseCors must be called before response caching
            app.UseRouting();
            app.UseResponseCaching();
            app.Use(async (ctx, next) =>
            {
                ctx.Response.GetTypedHeaders().CacheControl =
                    new CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10),
                    };
                ctx.Response.Headers[HeaderNames.Vary] = new [] {"Accept-Encoding", "User-Agent"};
                await next();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}").RequireAuthorization();
                endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}
