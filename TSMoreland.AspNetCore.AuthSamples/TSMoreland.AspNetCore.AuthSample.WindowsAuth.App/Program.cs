using System.Runtime.Versioning;
using Microsoft.AspNetCore.Server.HttpSys;
using TSMoreland.AspNetCore.AuthSample.WindowsAuth.App;

if (OperatingSystem.IsWindows())
{
    IHostBuilder builder = CreateHostBuilder(args);
    await builder.Build().RunAsync();
}

[SupportedOSPlatform("windows")]
static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            if (OperatingSystem.IsWindows())
            {
                /*
                 * see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/httpsys?view=aspnetcore-6.0
                 * The following is a snippet describing how to configure SSL certs
                 * 
                 * Register X.509 certificates on the server.
                 * Use the netsh.exe tool to register certificates for the app:
                 *   netsh http add sslcert ipport=<IP>:<PORT> certhash=<THUMBPRINT> appid="{<GUID>}"
                 */
                webBuilder.UseHttpSys(options =>
                {
                    options.AllowSynchronousIO = false;
                    options.Authentication.Schemes = AuthenticationSchemes.Negotiate;
                    options.Authentication.AllowAnonymous = false;
                    options.MaxConnections = null;
                    options.MaxRequestBodySize = 30000000;
                    options.UrlPrefixes.Add("http://localhost:5071");
                    options.UrlPrefixes.Add("https://localhost:7071");
                });
            }
            webBuilder.UseStartup<Startup>();
        });
}
