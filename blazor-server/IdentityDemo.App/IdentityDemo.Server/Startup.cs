//
// Copyright (c) 2023 Terry Moreland
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
using IdentityDemo.App.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityDemo.App.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace IdentityDemo.Server;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor();

        services.AddHttpClient<IWeatherForecastService, WeatherForecastService>(client =>
        {
            client.BaseAddress = new Uri(Configuration.GetSection("IdentityDemoApiAddress").Value); 
        });
            
        var idpOptions = Configuration
            .GetSection(IdentityProviderOptions.SectionName)
            .Get<IdentityProviderOptions>();
        var idpAuthenticationOptions = Configuration
            .GetSection(IdentityProviderAuthenticationOptions.SectionName)
            .Get<IdentityProviderAuthenticationOptions>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = idpOptions.Authority;
                options.ResponseType = idpAuthenticationOptions.ResponseType;
                foreach (var scope in idpAuthenticationOptions.ApiScopes)
                    options.Scope.Add(scope);
                options.SaveTokens = idpAuthenticationOptions.SaveTokens;
                options.GetClaimsFromUserInfoEndpoint = idpAuthenticationOptions.GetClaimsFromUserInfoEndPoint;
                // options.CallbackPath = ... not needed as this is stored in the Idp

                // see profile page for possible values
                options.TokenValidationParameters.NameClaimType = idpAuthenticationOptions.NameClaimType;

                // these two should definately be out in environment
                options.ClientId = idpOptions.ClientId;
                options.ClientSecret = idpOptions.ClientSecret;
            });


        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<ITokenManager, TokenManager>();
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
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
    }
}
