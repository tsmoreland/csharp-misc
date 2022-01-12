using TSMoreland.AspNetCore.RestartOnRequest;

Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseKestrel((context, kestrelOptions) =>
        {
            kestrelOptions
                .Configure(context.Configuration.GetSection("Kestrel"), reloadOnChange: true)
                .Endpoint("HTTPS", endpointOptions =>
                {
                    // let OS decide
                    endpointOptions.HttpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.None;
                });
            kestrelOptions.AddServerHeader = false;
        });

        webBuilder.UseStartup<Startup>();
    })
    .Build()
    .Run();
