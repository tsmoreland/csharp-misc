using System;

namespace TSMoreland.Interop.NetFramework.App;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        SimpleInProcessCOMLib.ISimpleObject instance = new SimpleInProcessCOMLib.SimpleObject();

        instance.Numeric = 37;
        (Guid id, string name, int numeric) = (instance.Id, instance.Name, instance.Numeric);

        Console.WriteLine($"Id = {id}, name = {name} numeric = {numeric}");
    }

    private static Type GetClassTypeFromId(Guid classId)
    {
        return Type.GetTypeFromCLSID(classId);
    }

}
