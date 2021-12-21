using System;
using SimpleInProcessCOMLib;

namespace TSMoreland.Interop.NetFramework.App;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        ISimpleObject instance = new SimpleObjectClass();

        instance.Numeric = 37;
        (Guid id, string name, int numeric) = (instance.Id, instance.Name, instance.Numeric);

        Console.WriteLine($"Id = {id}, name = {name} numeric = {numeric}");

        string result = instance.ConvertToString(Guid.Empty);
        Console.WriteLine(result);
    }

    private static Type GetClassTypeFromId(Guid classId)
    {
        return Type.GetTypeFromCLSID(classId);
    }

}
