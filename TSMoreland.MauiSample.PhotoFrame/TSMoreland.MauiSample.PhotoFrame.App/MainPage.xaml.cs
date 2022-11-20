using TSMoreland.MauiSample.PhotoFrame.App.Shared.Contracts;

namespace TSMoreland.MauiSample.PhotoFrame.App;

public partial class MainPage : ContentPage
{
    //private readonly IFolderPicker _folderPicker;
    //private readonly IImageProvider _imageProvider;

    //public MainPage(IFolderPicker folderPicker, IImageProvider imageProvider)
    public MainPage(IFolderPicker folderPicker)
    {
        InitializeComponent();
        //_folderPicker = folderPicker ?? throw new ArgumentNullException(nameof(folderPicker));
        //_imageProvider = imageProvider ?? throw new ArgumentNullException(nameof(imageProvider));

    }

    private async void SelectFolderButton_Clicked(object sender, EventArgs e)
    {
        await Task.CompletedTask;
        /*
        string? path = await _folderPicker.PickFolderAsync(default);
        if (path is not { Length: > 0 })
        {
            return;
        }

        await _imageProvider.InitializeFromFolder(path, true, true, default);
        await _imageProvider.NextAsync(null, default);
    */
    }
}

