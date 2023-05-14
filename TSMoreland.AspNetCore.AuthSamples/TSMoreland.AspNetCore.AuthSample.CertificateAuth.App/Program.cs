using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using TSMoreland.AspNetCore.AuthSample.CertificateAuth.App;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services
    .Configure<KestrelServerOptions>(options =>
    {
        options.AddServerHeader = false;
        options.ConfigureHttpsDefaults(httpsOptions =>
        {
            httpsOptions.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
            httpsOptions.CheckCertificateRevocation = false;
            httpsOptions.ClientCertificateValidation = (_, _, _) => true;
        });
    });

// certificate to use must include Oid 1.3.6.1.5.5.7.3.2 - client auth
builder.Services
    .AddScoped<ICertificateValidationService, CertificateValidationService>()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddAuthentication(static options =>
    {
        options.DefaultAuthenticateScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCertificate(options =>
    {
        options.AllowedCertificateTypes = CertificateTypes.All;
        options.Events = new CertificateAuthenticationEvents
        {
            OnAuthenticationFailed = context =>
            {
                _ = context;

                return Task.CompletedTask;
            },
            OnCertificateValidated = context =>
            {

                ICertificateValidationService validationService = context.HttpContext.RequestServices
                    .GetRequiredService<ICertificateValidationService>();

                if (!validationService.ValidateCertificate(context.ClientCertificate))
                {
                    return
                        Task.CompletedTask; // we could call context.Fail here but that may prevent subsequent auth handlers from being used, we'll experiment and find out
                }

                Claim[] claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, context.ClientCertificate.Subject, ClaimValueTypes.String,
                        context.Options.ClaimsIssuer),
                    new Claim(ClaimTypes.Name, context.ClientCertificate.Subject, ClaimValueTypes.String,
                        context.Options.ClaimsIssuer),
                };
                context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                context.Success();
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                _ = context;

                return Task.CompletedTask;
            }
        };
    });


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
