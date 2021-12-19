using System.Runtime.Versioning;

if (OperatingSystem.IsWindows())
{
    Console.WriteLine("Application not supported on any platform other than Windows");
}

Guid classId = new ("E3D3572D-9E25-4CF3-82F5-45B6F0035A82");
Type? classType = GetClassTypeFromId(classId);
if (classType == null)
{
    Console.WriteLine($"Class {classId} not found");
    return;
}
object? instance = Activator.CreateInstance(classType);
if (instance == null)
{
    Console.WriteLine($"Class {classType} returned null on creation");
    return;
}

dynamic dynamicInstance = instance;

string name = dynamicInstance.Name;
Console.WriteLine($"Name = {name}");

Guid temp;
object result = dynamicInstance.get_Id(out temp);

Guid id = dynamicInstance.Id;
Console.WriteLine($"Id = {id}");

Console.Out.WriteLine($"{id} {name}");


static Type? GetClassTypeFromId(Guid classId)
{
    if (OperatingSystem.IsWindows())
    {
        return GetWindowsClassTypeFromId(classId);
    }

    throw new NotSupportedException();
}

[SupportedOSPlatform("Windows")]
static Type? GetWindowsClassTypeFromId(Guid classId)
{
    return Type.GetTypeFromCLSID(classId);
}
