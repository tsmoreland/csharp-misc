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

        object? inprocesInstance = Activator.CreateInstance(classType);
        if (inprocesInstance == null)
        {
            Console.WriteLine($"Class {classType} returned null on creation");
            return;
        }

        dynamic dynamicInProcessInstance = inprocesInstance;

        string name = dynamicInProcessInstance.Name;
        Console.WriteLine($"Name = {name}");

        _ISimpleObjectEvents_OnPropertyChangedEventHandler callbackDelegate =
            new(InProcessInstance_OnPropertyChanged);
        IntPtr callback = Marshal.GetFunctionPointerForDelegate(callbackDelegate);
        dynamicInProcessInstance.add_OnPropertyChanged(callback);
        dynamicInProcessInstance.Numeric = 42;
        int numeric = dynamicInProcessInstance.Numeric;
        Console.WriteLine($"Numeric = {numeric}");

        string description = dynamicInProcessInstance.Description;
        Console.WriteLine(description);

        // Beginning of Out of Process, should move elsewhere 


        if (inprocesInstance is ISimpleObject simpleObject)
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

        if (inprocesInstance is IMinimalSimpleObject simpleObject2)
        {
            Guid instanceId = simpleObject2.Id;
            Console.WriteLine($"Id from manually implemented C# interface {instanceId}");
        }

        try
        {
            string converted = dynamicInProcessInstance.ConvertToString(Guid.Empty);
            Console.WriteLine($"Converted {converted}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }


        if (inprocesInstance is IDispatch dispatch)
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


        Guid id = dynamicInProcessInstance.Id;
        Console.WriteLine($"Id = {id}");

        Console.Out.WriteLine($"{id} {name}");
    }

    private static void InProcessInstance_OnPropertyChanged(string propertyName)
    {
        Console.WriteLine($"'{propertyName}' has changed");
    }
}
