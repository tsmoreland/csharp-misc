using TSMoreland.ObjectTracker.Data.Abstractions.Entities;

namespace TSMoreland.ObjectTracker.Data.Abstractions.ViewModels;

public readonly record struct ObjectViewModel(int Id, string Name)
{
    public static ObjectViewModel ConvertFrom(ObjectEntity entity)
    {
        return new ObjectViewModel(entity.Id, entity.Name);
    }

}