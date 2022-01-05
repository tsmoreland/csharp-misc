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
#if NET6_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace TSMoreland.Interop.SimpleObjectCOMProxy;

public sealed class SimpleOopObjectFacade : ISimpleOopObjectFacade
{
    private readonly SimpleOOPObjectEventsProvider _provider;
    private readonly dynamic _object;

    public SimpleOopObjectFacade()
    {
        Guid classId = new("972b85e9-b7c9-467e-9c38-da5423ebcb1e");
        Type? classType = GetClassTypeFromId(classId);

        if (classType == null)
        {
            throw new COMException("Class Type not found");
        }

        _object = Activator.CreateInstance(classType) ?? throw new COMException("Class not found");
        _provider = new SimpleOOPObjectEventsProvider(_object);
        
    }

    public object Object => _object;

    private static Type? GetClassTypeFromId(Guid classId)
    {
#if NET6_0_OR_GREATER
        if (OperatingSystem.IsWindows())
        {
            return GetWindowsClassTypeFromId(classId);
        }
        throw new NotSupportedException();
#else
        return GetWindowsClassTypeFromId(classId);
#endif
    }

#if NET6_0_OR_GREATER
    [SupportedOSPlatform("Windows")]
#endif
    private static Type? GetWindowsClassTypeFromId(Guid classId)
    {
        return Type.GetTypeFromCLSID(classId);
    }

    public event OOPOnPropertyChangedHandler PropertyChanged
    {
        add => _provider.add_OOPOnPropertyChanged(value);
        remove => _provider.remove_OOPOnPropertyChanged(value);
    }


    public string Name => _object.Name;

    public int Numeric
    {
        get => _object.Numeric;
        set => _object.Numeric = value;
    }

    public Guid Id
    {
        get
        {
            try
            {
                // this will throw an exception because dynamic seems to rely on IDispatch to call these properties/methods
                // while MIDL can work some dark magic to support GUID IDispatch requires output/input but a valid COM Variant type
                // for which GUID is not
                return _object.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Guid.Empty;
            }
        }
    }

    public string ConvertToString(Guid input)
    {
        try
        {
            // as above this won't work because we can't pass in or get Guid in response as they are not valid COM Variant types
            return _object.ConvertToString(input);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return string.Empty;
        }
    }

    /// <summary>
    /// Description which comes from ISimpleObject2 
    /// </summary>
    public string Description => _object.Description;


    #region IDisposable
    ///<summary>Finalize</summary>
    ~SimpleOopObjectFacade() => Dispose(false);

    ///<summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        Dispose(true);
        System.GC.SuppressFinalize(this);
    }

    ///<summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    ///<param name="disposing">if <c>true</c> then release managed resources in addition to unmanaged</param>
    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _provider.Dispose();
        }
    }
    #endregion

}
