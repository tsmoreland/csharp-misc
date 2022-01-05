//
// Copyright © 2022 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using TSMoreland.Interop.SimpleObjectCOMProxy;

namespace TSMoreland.Interop.App;

internal static class OutOfProcessTest
{
    public static void Verify()
    {
        VerifySimpleObjectFacade();

        Guid classId = new ("972B85E9-B7C9-467E-9C38-DA5423EBCB1E");
        Type? classType = TypeHelper.GetClassTypeFromId(classId);
        if (classType == null)
        {
            Console.WriteLine($"Class {classId} not found");
            return;
        }
        object? @object = Activator.CreateInstance(classType);
        if (@object == null)
        {
            Console.WriteLine($"Class {classType} returned null on creation");
            return;
        }
        dynamic instance = @object;

        string name = instance.Name;
        Console.WriteLine($"(OOP) Name = {name}");

        instance.Numeric = 42;
        int numeric = instance.Numeric;
        Console.WriteLine($"(OOP) Numeric = {numeric}");

        string description = instance.Description;
        Console.WriteLine($"(OOP) Description = {description}");

    }

    private static void VerifySimpleObjectFacade()
    {
        //using ISimpleOopObjectFacade facade = new SimpleOopObjectFacade();
        using SimpleOopObjectFacade facade = new ();

        string name = facade.Name;
        Console.WriteLine($"Name = {name}");

        object comObject = facade.Object;
        if (comObject is IConnectionPointContainer container)
        {
            //container.EnumConnections(out IEnumConnections connections);
            container.EnumConnectionPoints(out IEnumConnectionPoints connections);

            connections.Reset();
            IConnectionPoint[] data = new IConnectionPoint[1];
            IntPtr ptr = IntPtr.Zero;
            if (0 == connections.Next(1, data!, ptr))
            {
                IConnectionPoint point = data[0];
                Type type = point.GetType();

                Console.WriteLine("Connection Point type: " + type.FullName);

            }
        }


        facade.PropertyChanged += Facade_PropertyChanged;
        facade.Numeric = 24;
        int numeric = facade.Numeric;
        Console.WriteLine($"Numeric = {numeric}");
        facade.PropertyChanged -= Facade_PropertyChanged;

        string description = facade.Description;
        Console.WriteLine(description);
    }

    private static void Facade_PropertyChanged([In, MarshalAs(System.Runtime.InteropServices.UnmanagedType.BStr)] string propertyName)
    {
        Console.WriteLine("Property changed: " + propertyName);
    }
}
