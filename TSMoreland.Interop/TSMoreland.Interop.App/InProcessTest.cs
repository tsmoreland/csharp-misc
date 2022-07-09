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

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.CSharp.RuntimeBinder;
using TSMoreland.Interop.SimpleObjectCOMProxy;

namespace TSMoreland.Interop.App;

internal static class InProcessTest
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

    public static void Verify()
    {
        VerifySimpleObjectFacade();

        if (!TryCreateComObject(out object? @object))
        {
            return;
        }

        dynamic instance = @object!;

        string name = instance.Name;
        Console.WriteLine($"Name = {name}");

        instance.Numeric = 24;
        int numeric = instance.Numeric;
        Console.WriteLine($"Numeric = {numeric}");

        MinimalEventTest(@object!);

        ConnectionPointContainerTest(instance);

        string description = instance.Description;
        Console.WriteLine(description);

        // Beginning of Out of Process, should move elsewhere

        SimpleObjectTest(@object!);
        MinimalSimpleObjectTest(@object!);
        TryAccessGuidMethod(instance);

        DispatchTest(@object!);


        Guid id = instance.Id;
        Console.WriteLine($"Id = {id}");

        Console.Out.WriteLine($"{id} {name}");
    }

    private static void VerifySimpleObjectFacade()
    {
        using ISimpleObjectFacade facade = new SimpleObjectFacade();

        string name = facade.Name;
        Console.WriteLine($"Name = {name}");

        facade.PropertyChanged += InProcessInstance_OnPropertyChanged;
        facade.Numeric = 24;
        int numeric = facade.Numeric;
        Console.WriteLine($"Numeric = {numeric}");
        facade.PropertyChanged -= InProcessInstance_OnPropertyChanged;

        string description = facade.Description;
        Console.WriteLine(description);
    }

    private static bool TryCreateComObject(out object? @object)
    {
        Guid classId = new("E3D3572D-9E25-4CF3-82F5-45B6F0035A82");
        Type? classType = TypeHelper.GetClassTypeFromId(classId);

        @object = null;

        if (classType == null)
        {
            Console.WriteLine($"Class {classId} not found");
            return false;
        }

        @object = Activator.CreateInstance(classType);
        if (@object == null)
        {
            Console.WriteLine($"Class {classType} returned null on creation");
            return false;
        }

        return true;
    }

    /// <summary>
    /// the following attempt does not work, left for future reference but not to be seen as
    /// a working solution
    /// </summary>
    private static void ConnectionPointContainerTest(dynamic instance)
    {
        if (instance is IConnectionPointContainer container)
        {
            container.EnumConnectionPoints(out IEnumConnectionPoints connectionPoints);

            IConnectionPoint[] points = new IConnectionPoint[10];

            IntPtr countPtr = IntPtr.Zero;
            int count = 0;
            try
            {
                countPtr = Marshal.AllocHGlobal(sizeof(int));
                connectionPoints.Next(10, points, countPtr);
                count = Marshal.ReadInt32(countPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(countPtr);
            }

            for (int i = 0; i < count; i++)
            {
                IConnectionPoint point = points[i];
                if (point == null!)
                {
                    continue;
                }

                point.GetConnectionInterface(out Guid iid);
                if (iid == new Guid("71A4D526-4FAD-4D4B-8A6E-78AFCABD7F63"))
                {
                    // ...
                }
            }
        }
        _ISimpleObjectEvents_OnPropertyChangedEventHandler callbackDelegate =
            new(InProcessInstance_OnPropertyChanged);

        int added = 0;
        try
        {
            instance.add_PropertyChanged(callbackDelegate);
            added = 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        IntPtr callback = Marshal.GetFunctionPointerForDelegate(callbackDelegate);

        if (added == 0)
        {
            try
            {
                instance.add_PropertyChanged(callback);
                added = 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        instance.add_OnPropertyChanged(callback);
        instance.Numeric = 42;
        int numeric = instance.Numeric;
        Console.WriteLine($"Numeric = {numeric}");
        if (added != 0)
        {
            try
            {
                if (added == 1)
                {
                    instance.remove_PropertyChanged(callbackDelegate);
                }

                if (added == 2)
                {
                    instance.remove_PropertyChanged(callback);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


    }

    /// <summary>
    /// Another failed attempt, dynamic uses IDispatch to make these calls which means all
    /// the types used must be a valid form of COM Variant - GUID Is not
    /// </summary>
    private static void TryAccessGuidMethod(dynamic instance)
    {
        try
        {
            string converted = instance.ConvertToString(Guid.Empty);
            Console.WriteLine($"Converted {converted}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    /// <summary>
    /// this simply verifies that we can access members of ISimpleObject2 using an interfade reference
    /// to ISimpleObject - because it's using IDispatch it'll eventually see what the instance
    /// is actually capable of
    /// </summary>
    private static void SimpleObjectTest(object @object)
    {
        if (@object is not ISimpleObject simpleObject)
        {
            return;
        }

        dynamic dynamicSimpleObject = simpleObject;

        try
        {
            // theory here is that even though we started with an ISimpleObject we can
            // still access ISimpleObject2 when using a dynamic
            string dynamicDescription = dynamicSimpleObject.Description;
            Console.WriteLine(dynamicDescription);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }


        Guid instanceId = simpleObject.Id;
        Console.WriteLine($"Id from manually implemented C# interface {instanceId}");
        string converted = simpleObject.ConvertToString(Guid.Empty);

        Console.WriteLine($"Converted {converted}");

    }

    /// <summary>
    /// experient to see how small a 'working' interface for the COM type would need to be
    /// it does pass the first check but the second will throw an exception because we can't
    /// work with GUID
    /// </summary>
    private static void MinimalSimpleObjectTest(object @object)
    {
        if (@object is not IMinimalSimpleObject simpleObject2)
        {
            return;
        }

        Guid instanceId = simpleObject2.Id;
        Console.WriteLine($"Id from manually implemented C# interface {instanceId}");

    }

    /// <summary>
    /// testing ways to work with events, this was the ground work for the generator
    /// </summary>
    private static void MinimalEventTest(object @object)
    {
        if (@object is not ISimpleObjectEventsEvent eventProducer)
        {
            Console.WriteLine("object does not implement event producer or interface is incorrect");
            return;
        }

        dynamic instance = @object;

        SimpleObjectEventsPropertyChangedEventHandler callbackDelegate =
            new(InProcessInstance_OnPropertyChanged);
        try
        {
            eventProducer.add_OnPropertyChanged(callbackDelegate);
            instance.Numeric = 27;
            Console.WriteLine(instance.Numeric);
            eventProducer.remove_OnPropertyChanged(callbackDelegate);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        try
        {
            using var provider = new SimpleComWrappers.ManualSimpleObjectEventProvider(@object);
            provider.PropertyChanged += InProcessInstance_OnPropertyChanged;
            instance.Numeric = 53;
            Console.WriteLine(instance.Numeric);
            provider.PropertyChanged -= InProcessInstance_OnPropertyChanged;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        SimpleObjectCOMProxy.OnPropertyChangedHandler handler =
            new (InProcessInstance_OnPropertyChanged);
        try
        {
            Console.WriteLine("Generated Event Provider:");
            using var provider = new SimpleObjectCOMProxy.SimpleObjectEventsProvider(@object);
            provider.add_OnPropertyChanged(handler);
            instance.Numeric = 42;
            Console.WriteLine(instance.Numeric);
            provider.remove_OnPropertyChanged(handler);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

    }

    /// <summary>
    /// attempts to use IDispatch directly to get access to GUID but of course we can't
    /// because that requires the inputs/outputs to be valid COM variants
    /// </summary>
    private static void DispatchTest(object @object)
    {
        if (@object is not IDispatch dispatch)
        {
            return;
        }

        Guid empty = Guid.Empty;

        string[] properties = new[] { "Id" };
        foreach (string property in properties)
        {
            string propertyName = property;

            int result = dispatch.GetDispId(ref empty, ref propertyName, 1, 2048, out int dispId);

            if (result == 0)
            {
                Console.WriteLine($"Disp Id: {dispId}");

                DISPPARAMS dispParams = new()
                {
                    cArgs = 0, cNamedArgs = 0, rgdispidNamedArgs = IntPtr.Zero, rgvarg = IntPtr.Zero,
                };
                EXCEPINFO excepInfo = new();

                try
                {
                    result = dispatch.Invoke(dispId, ref empty, 2048, (ushort)INVOKEKIND.INVOKE_PROPERTYGET,
                        ref dispParams,
                        out object variant, ref excepInfo,
                        out uint argErr);

                    if (result == 0)
                    {
                        Console.WriteLine($"Success: {variant}");
                    }
                    else
                    {
                        Console.WriteLine($"Result = {result:X}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine($"Result = {result}");
            }
        }
    }


    /// <summary>
    /// the handler used in all evnet tests
    /// </summary>
    private static void InProcessInstance_OnPropertyChanged(string propertyName)
    {
        Console.WriteLine($"'{propertyName}' has changed");
    }
}
