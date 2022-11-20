using Microsoft.Extensions.Logging;
using TSMoreland.MauiSample.PhotoFrame.App.Services;
using TSMoreland.MauiSample.PhotoFrame.App.Shared.Contracts;

namespace TSMoreland.MauiSample.PhotoFrame.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddTransient<IImageProvider, ImageProvider>();
#if WINDOWS
		builder.Services.AddTransient<IFolderPicker, Platforms.Windows.FolderPicker>();
#elif ANDROID
        // TODO...
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
