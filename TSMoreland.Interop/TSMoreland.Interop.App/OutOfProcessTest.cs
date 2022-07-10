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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.CSharp.RuntimeBinder;
using TSMoreland.Interop.SimpleObjectCOMProxy;

namespace TSMoreland.Interop.App;

internal static class OutOfProcessTest
{

    public static void VerifyMemoryUse()
    {
        string? line;

        Console.WriteLine("Press enter to stop");
        List<object?> items = new();
        List<CallSite> callsites = new();
        do
        {
            if (!TryCreateComObject(out object? @object))
            {
                return;
            }


            CallSite<Func<CallSite, object?, object?>> nameCallSite = CallSite<Func<CallSite, object?, object?>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Name", typeof (InProcessTest), new []
            {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string?) null)
            }));
            callsites.Add(nameCallSite);
            object? maybeName = nameCallSite.Target(nameCallSite, @object);
            if (maybeName is string name)
            {
                Console.WriteLine($"name from callsite '{name}'");
            }
            else
            {
                name = "unknown";
            }


            CallSite<Func<CallSite, object?, int, object?>> setNumericCallSite = CallSite<Func<CallSite, object?, int, object?>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Numeric", typeof (InProcessTest), new []
            {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string?) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string?) null)
            }));
            callsites.Add(setNumericCallSite);
            Console.WriteLine("Set Numeric");
            setNumericCallSite.Target(setNumericCallSite, @object, 24);

            CallSite<Func<CallSite, object?, object?>> numericCallSite = CallSite<Func<CallSite, object?, object?>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Numeric", typeof (InProcessTest), new []
            {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string?) null)
            }));
            callsites.Add(numericCallSite);
            object? numeric = numericCallSite.Target(numericCallSite, @object);
            if (numeric is int numericValue)
            {
                Console.WriteLine($"numeric from callsite {numericValue}");
            }


            CallSite<Func<CallSite, object?, string, object?>> toUpperCallsite = CallSite<Func<CallSite, object?, string, object?>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "ToUpper", null, typeof(InProcessTest), new[]
            {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string?) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string?) null)
            }));
            callsites.Add(toUpperCallsite);
            object? maybeUpperName = toUpperCallsite.Target(toUpperCallsite, @object, name);
            if (maybeName is string upperName)
            {
                Console.WriteLine($"Name (uppercase) = {upperName}");
            }

            line = Console.ReadLine();
            items.Add(@object);

        } while (line?.ToUpperInvariant() != "QUIT");

        foreach (IDisposable item in items.OfType<IDisposable>())
        {
            item.Dispose(); // it won't but we just to keep these objects alive until here to test for leaks
        }

        items.Clear();
        Console.WriteLine("Present enter to clear call sites");
        _ = Console.ReadLine();
        callsites.Clear();
        Console.WriteLine("Present enter to clear call exit");
        _ = Console.ReadLine();
    }

    public static void VerifyMemoryUseWithDynamic()
    {
        string? line;
        Console.WriteLine("Press enter to stop");
        List<object?> items = new();
        List<CallSite> callsites = new();
        do
        {
            if (!TryCreateComObject(out object? @object))
            {
                return;
            }

            dynamic instance = @object!;
            string name = instance.Name;
            Console.WriteLine($"Name = {name}");

            instance.Numeric = 24;
            int value = instance.Numeric;
            Console.WriteLine($"Numeric = {value}");

            string upperName = instance.ToUpper(name);
            Console.WriteLine($"Name (uppercase) = {upperName}");

            line = Console.ReadLine();
            items.Add(@object);
            @object = null;

        } while (line?.ToUpperInvariant() != "QUIT");

        items.Clear();
        Console.WriteLine("Present enter to clear call sites");
        _ = Console.ReadLine();
        callsites.Clear();
        Console.WriteLine("Present enter to clear call exit");
        _ = Console.ReadLine();

    }

    private static bool TryCreateComObject([NotNullWhen(true)] out object? @object)
    {
        Guid classId = Guid.Parse("972B85E9-B7C9-467E-9C38-DA5423EBCB1E");
        Type? classType = TypeHelper.GetClassTypeFromId(classId);
        if (classType == null)
        {
            Console.WriteLine($"Class {classId} not found");
            @object = null;
            return false;
        }
        @object = Activator.CreateInstance(classType);
        return true;
    }

    public static void Verify()
    {
        VerifySimpleObjectFacade();

        Guid classId = Guid.Parse("972B85E9-B7C9-467E-9C38-DA5423EBCB1E");
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
        using ISimpleOopObjectFacade facade = new SimpleOopObjectFacade();
        const int ok = 0;

        string name = facade.Name;
        Console.WriteLine($"Name = {name}");

        object comObject = facade.Object;
        if (comObject is IConnectionPointContainer container)
        {
            container.EnumConnectionPoints(out IEnumConnectionPoints connections);
            connections.Reset();
            IConnectionPoint[] data = new IConnectionPoint[1];
            IntPtr ptr = IntPtr.Zero;
            if (connections.Next(1, data!, ptr) == ok)
            {
                IConnectionPoint point = data[0];
                point.GetConnectionInterface(out Guid connectionInterface);
                Console.WriteLine($"Connection Point interface: {connectionInterface}");
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
