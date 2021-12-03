using Application = Microsoft.Maui.Controls.Application;

namespace TSMoreland.Maui.PhotoViewer;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }
}