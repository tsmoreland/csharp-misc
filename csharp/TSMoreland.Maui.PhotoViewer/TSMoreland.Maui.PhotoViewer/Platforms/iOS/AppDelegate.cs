using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace TSMoreland.Maui.PhotoViewer
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	}
}