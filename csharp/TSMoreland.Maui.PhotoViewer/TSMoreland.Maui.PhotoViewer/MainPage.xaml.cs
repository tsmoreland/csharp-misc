using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

namespace TSMoreland.Maui.PhotoViewer;

public partial class MainPage 
{
    private readonly List<string> _files = new();

#   pragma warning disable CS8618 // controls are not seen as initialized so trigger this warning
    public MainPage()
    {
        InitializeComponent();

    }
#   pragma warning restore CS8618

    private async void OnFolderSelectorClicked(object sender, EventArgs e)
    {
        var options = new PickOptions
        {
            FileTypes = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.UWP, new [] { "*.jpg", "*.jpeg", "*.gif" } },
                    { DevicePlatform.macOS, new [] { "*.jpg", "*.jpeg", "*.gif" } },
                    { DevicePlatform.Android, new [] { "*.jpg", "*.jpeg", "*.gif" } },
                })
        };

        var files = await FilePicker.PickMultipleAsync(options);
        _files.Clear();
        _files.AddRange(files.Select(f => f.FullPath));
    }
}