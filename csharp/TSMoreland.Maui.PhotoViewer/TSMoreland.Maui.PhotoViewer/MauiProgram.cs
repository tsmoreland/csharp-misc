using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;

namespace TSMoreland.Maui.PhotoViewer
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .UseMauiApp<App>()
                .Host
                .ConfigureAppConfiguration((app, config) =>
                    config
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddUserSecrets<App>()
                        .AddEnvironmentVariables());

			return builder.Build();
		}
	}
}