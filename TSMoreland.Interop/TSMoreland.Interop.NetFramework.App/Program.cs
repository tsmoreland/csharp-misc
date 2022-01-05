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

using System;
using SimpleInProcessCOMLib;
using SimpleOutOfProcessCOMLib;
using TSMoreland.Interop.SimpleObjectCOMProxy;

namespace TSMoreland.Interop.NetFramework.App;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        InProcessTest();
        OutOfProcessTest();
    }

    private static void InProcessTest()
    {
        Console.WriteLine("=============== Old style generated interop =============");
        SimpleObject instance = new SimpleObjectClass();

        instance.OnPropertyChanged += Instance_OnPropertyChanged;

        instance.Numeric = 37;
        (Guid id, string name, int numeric) = (instance.Id, instance.Name, instance.Numeric);
        instance.OnPropertyChanged -= Instance_OnPropertyChanged;

        Console.WriteLine($"Id = {id}, name = {name} numeric = {numeric}");

        string result = instance.ConvertToString(Guid.Empty);
        Console.WriteLine(result);

        Console.WriteLine("=============== Facade Approach =============");
        using ISimpleObjectFacade facade = new SimpleObjectFacade();
        facade.PropertyChanged += Instance_OnPropertyChanged;

        facade.Numeric = 96;
        // skip id because we know it'll throw an exception, without knowing the exact interface we rely
        // on IDispatch which only works with variant types
        (name, numeric) = (facade.Name, facade.Numeric);
        facade.PropertyChanged -= Instance_OnPropertyChanged;

        Console.WriteLine($"name = {name} numeric = {numeric}");
    }

    private static void OutOfProcessTest()
    {
        Console.WriteLine("=============== Old style generated interop =============");
        SimpleOOPObject instance = new SimpleOOPObjectClass();

        instance.OnPropertyChanged += Instance_OnPropertyChanged;

        instance.Numeric = 37;
        (Guid id, string name, int numeric) = (instance.Id, instance.Name, instance.Numeric);

        Console.WriteLine($"Id = {id}, name = {name} numeric = {numeric}");
        instance.OnPropertyChanged -= Instance_OnPropertyChanged;

        try
        {
            Console.WriteLine("=============== Facade Approach =============");
            using ISimpleOopObjectFacade facade = new SimpleOopObjectFacade();
            facade.PropertyChanged += Instance_OnPropertyChanged;

            instance.Numeric = 96;
            // skip id because we know it'll throw an exception, without knowing the exact interface we rely
            // on IDispatch which only works with variant types
            (name, numeric) = (facade.Name, facade.Numeric);
            facade.PropertyChanged -= Instance_OnPropertyChanged;

            Console.WriteLine($"name = {name} numeric = {numeric}");
        }
        catch (Exception ex)
        {
            // still trying to get the connection point worked out, something's not right in the setup of generated code
            Console.WriteLine(ex);
        }
    }


    private static void Instance_OnPropertyChanged(string propertyName)
    {
        Console.WriteLine($"'{propertyName}' has changed");
    }

}
