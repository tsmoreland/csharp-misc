//
// Copyright © 2021 Terry Moreland
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

namespace TSMoreland.Interop.App;

internal static class InProcessTest
{

    public static void Verify()
    {
        Guid classId = new("E3D3572D-9E25-4CF3-82F5-45B6F0035A82");
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
        Console.WriteLine($"Name = {name}");

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


        string description = instance.Description;
        Console.WriteLine(description);

        // Beginning of Out of Process, should move elsewhere 


        if (@object is ISimpleObject simpleObject)
        {
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

        if (@object is IMinimalSimpleObject simpleObject2)
        {
            Guid instanceId = simpleObject2.Id;
            Console.WriteLine($"Id from manually implemented C# interface {instanceId}");
        }

        try
        {
            string converted = instance.ConvertToString(Guid.Empty);
            Console.WriteLine($"Converted {converted}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }


        if (@object is IDispatch dispatch)
        {
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


        Guid id = instance.Id;
        Console.WriteLine($"Id = {id}");

        Console.Out.WriteLine($"{id} {name}");
    }

    private static void InProcessInstance_OnPropertyChanged(string propertyName)
    {
        Console.WriteLine($"'{propertyName}' has changed");
    }
}
