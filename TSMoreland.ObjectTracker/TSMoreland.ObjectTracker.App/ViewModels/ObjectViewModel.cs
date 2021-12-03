using TSMoreland.ObjectTracker.Core;

namespace TSMoreland.ObjectTracker.App.ViewModels;

public sealed record ObjectViewModel(int Id, string Name, IReadOnlyList<LogViewModel> Logs)
{
    public static ObjectViewModel ConvertFrom(ObjectModel model)
    {
        var (id, name, logs) = model;
        return new ObjectViewModel(id, name, logs.Select(LogViewModel.ConvertFrom).ToList());
    }

}