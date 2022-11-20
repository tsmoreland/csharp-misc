using TSMoreland.MauiSample.PhotoFrame.App.Shared.Contracts;

namespace TSMoreland.MauiSample.PhotoFrame.App;

public partial class MainPage : ContentPage
{
    private readonly IFolderPicker _folderPicker;
    int _count = 0;

    public MainPage(IFolderPicker folderPicker)
    {
        InitializeComponent();
        _folderPicker = folderPicker ?? throw new ArgumentNullException(nameof(folderPicker));
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        _count++;

        if (_count == 1)
            CounterBtn.Text = $"Clicked {_count} time";
        else
            CounterBtn.Text = $"Clicked {_count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private async void SelectFolderButton_Clicked(object sender, EventArgs e)
    {
        string? path = await _folderPicker.PickFolderAsync(default);
        if (path is not { Length: > 0 })
        {
            return;
        }


        
    }
}

