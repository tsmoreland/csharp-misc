using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Certificate;
using TSMoreland.AspNetCore.AuthSample.CertificateAuth.App;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// certificate to use must include Oid 1.3.6.1.5.5.7.3.2 - client auth

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(options =>
    {
        options.AllowedCertificateTypes = CertificateTypes.All;
        options.Events = new CertificateAuthenticationEvents
        {
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

app.UseAuthorization();

app.MapControllers();

app.Run();
