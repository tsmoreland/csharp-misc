
using System.ComponentModel.DataAnnotations;
using TSMoreland.ObjectTracker.Core;

namespace TSMoreland.ObjectTracker.App.ViewModels
{
    public sealed record LogViewModel([property: Required] int Level, [property: Required] string Name)
    {

        public static LogViewModel ConvertFrom(LogModel model)
        {
            var (level, name) = model;
            return new LogViewModel(level, name);
        }

        public LogModel ToLogModel()
        {
            return new LogModel(Level, Name);
        }
    }
}
