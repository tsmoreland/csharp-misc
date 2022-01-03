﻿//
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

namespace TSMoreland.Interop.NetFramework.App;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        SimpleObject instance = new SimpleObjectClass();

        instance.OnPropertyChanged += Instance_OnPropertyChanged;

        instance.Numeric = 37;
        (Guid id, string name, int numeric) = (instance.Id, instance.Name, instance.Numeric);

        Console.WriteLine($"Id = {id}, name = {name} numeric = {numeric}");

        string result = instance.ConvertToString(Guid.Empty);
        Console.WriteLine(result);
    }

    private static void Instance_OnPropertyChanged(string propertyName)
    {
        Console.WriteLine($"'{propertyName}' has changed");
    }

    private static Type GetClassTypeFromId(Guid classId)
    {
        return Type.GetTypeFromCLSID(classId);
    }

}
