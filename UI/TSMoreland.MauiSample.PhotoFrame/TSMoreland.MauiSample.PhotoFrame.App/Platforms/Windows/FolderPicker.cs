using Windows.Storage;
using TSMoreland.MauiSample.PhotoFrame.App.Shared.Contracts;
using WindowsFolderPicker = Windows.Storage.Pickers.FolderPicker;

namespace TSMoreland.MauiSample.PhotoFrame.App.Platforms.Windows;

/// <summary>
/// <see href="https://github.com/jfversluis/MauiFolderPickerSample/blob/main/MauiFolderPickerSample/Platforms/Windows/FolderPicker.cs">
/// .NET MAUI Folder Picker Sample
/// </see>
/// </summary>
public sealed class FolderPicker : IFolderPicker
{
    /// <inheritdoc />
    public async Task<string?> PickFolderAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        WindowsFolderPicker picker = new();
        picker.FileTypeFilter.Add("*");

        if (Application.Current?.Windows[0].Handler.PlatformView is not MauiWinUIWindow uiWindow)
        {
            throw new PlatformNotSupportedException();
        }

        IntPtr windowHandle = uiWindow.WindowHandle;

        WinRT.Interop.InitializeWithWindow.Initialize(picker, windowHandle);

        StorageFolder? result = await picker.PickSingleFolderAsync();

        return result?.Path;
    }
}
