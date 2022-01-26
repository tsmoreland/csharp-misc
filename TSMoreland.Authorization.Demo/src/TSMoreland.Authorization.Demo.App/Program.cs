//
// Copyright © 2022 Terry Moreland
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

using TSMoreland.Authorization.Demo.LocalUsers.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Serilog;
using TSMoreland.Authorization.Demo.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
LoggerConfiguration loggerConfiguration = new ();
loggerConfiguration.ReadFrom.Configuration(builder.Configuration);
builder.Logging.AddSerilog(loggerConfiguration.CreateLogger());

builder.WebHost
    .ConfigureKestrel(kestrelServerOptions =>
    {
        kestrelServerOptions.AddServerHeader = false;
    });

IServiceCollection services = builder.Services;

const string defaultPolicy = ""; // TODO - this policy should match the default scheme
const string requiredScope = ""; // this should match some scope we expected to fin
const string defaultAuthenticationScheme = ""; // TODO
const string defaultChallengeScheme = "";

services
    .AddControllers();

services
    .Configure<DataProtectionTokenProviderOptions>(tokenProviderOptions =>
    {
        tokenProviderOptions.TokenLifespan = TimeSpan.FromHours(1);
    })
    .AddAuthorization(authorizationOptions =>
    {
        authorizationOptions.AddPolicy(defaultPolicy, policy =>
        {
            // this will only work if we are requiring one scope, similar idea
            // for multiple but we'd need to check if all required items are in this collection
            // we'd likely need to make it a list then something like requiredScopes.All(s => scopes.Contains)
            policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context => 
                    context.User.Claims
                        .Where(c => c.Type.Equals("scope"))
                        .SelectMany(c => c.Value.Split(' '))
                        .Select(s => s.ToUpperInvariant())
                        .Contains(requiredScope.ToUpperInvariant()));
        });

    })
    .AddLocalUsersWithIdentity(identityOptions =>
    {
        // not a secure choice of options
        identityOptions.SignIn.RequireConfirmedPhoneNumber = false;
        identityOptions.SignIn.RequireConfirmedEmail = false;
        identityOptions.SignIn.RequireConfirmedAccount = false;

        identityOptions.Password.RequireUppercase = false;
        identityOptions.Password.RequireLowercase = true;
        identityOptions.Password.RequireDigit = true;
        identityOptions.Password.RequireNonAlphanumeric = false;
        identityOptions.Password.RequiredLength = 8;
    })
    .AddDefaultTokenProviders();

SecurityHeadersOptions securityHeadersOptions = builder.Configuration
    .GetSection(SecurityHeadersOptions.SectionName)
    .Get<SecurityHeadersOptions>();

if (securityHeadersOptions?.EnableCors is true)
{
    services
        .AddCors(corsOptions =>
        {
            corsOptions.AddPolicy("AllowAllOrigins", policy =>
                policy
                    .AllowAnyOrigin()
                    .WithMethods("GET", "PUT", "POST", "DELETE")
                    .WithHeaders("Content-Type", "Accept", "Authorization", "Accept-Encoding")
                    .DisallowCredentials());
            corsOptions.AddPolicy("RestrictedOrigins", policy =>
                policy
                    .WithOrigins(securityHeadersOptions.AllowedOrigins.ToArray())
                    .WithMethods("GET", "PUT", "POST", "DELETE")
                    .WithHeaders("Content-Type", "Accept", "Authorization", "Accept-Encoding")
                    .DisallowCredentials());
        });
}

services
    .AddSecurityHeaders()
    .AddAuthentication(authenticationOptions =>
    {
        // update with something, anything once we have something
        authenticationOptions.DefaultAuthenticateScheme = defaultAuthenticationScheme;
        authenticationOptions.DefaultChallengeScheme = defaultChallengeScheme;
    });

WebApplication app = builder.Build();

IHostEnvironment environment = app.Services.GetRequiredService<IHostEnvironment>();

app.UseSecurityHeaders();
app.UseCors("AllowAllOrigins");

if (environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseHsts();
}

app.MapGet("/", () => "Hello World!");
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
