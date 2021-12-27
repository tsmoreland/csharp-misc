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

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace TSMoreland.Interop.App.SimpleComWrappers;

public sealed class SimpleObjectEventProvider : SimpleObjectEventProvider.IComEvents, IDisposable
{
    private readonly IConnectionPointContainer _connectionPointContainer;
    private List<SinkHelper>? _sinkHelpers;
    private IConnectionPoint? _connectionPoint;
    private readonly object _lock = new();

    public SimpleObjectEventProvider(object? @object)
    {
        if (@object is not IConnectionPointContainer connectionPointContainer)
        {
            throw new ArgumentException("provided object is not a connection point container", nameof(@object));
        }
        _connectionPointContainer = connectionPointContainer;
    }

    public event ComEventHandler PropertyChanged
    {
        add => add_OnPropertyChanged(value);
        remove => remove_OnPropertyChanged(value);
    }

    [ComVisible(false)]
    public delegate void ComEventHandler([MarshalAs(UnmanagedType.BStr), In] string propertyName);

    [ClassInterface(ClassInterfaceType.None)]
    [TypeLibType(TypeLibTypeFlags.FHidden)]
    public sealed class SinkHelper : IComEventHandler
    {
        internal SinkHelper()
        {
        }

        public ComEventHandler? Delegate { get; set; } = null;
        public int Cookie { get; set; } = 0;

        /// <inheritdoc />
        public void OnPropertyChanged([In] string propertyName) =>
            Delegate?.Invoke(propertyName);
    }


    [TypeLibType(4096)]
    [Guid("71A4D526-4FAD-4D4B-8A6E-78AFCABD7F63")]
    [InterfaceType(2)]
    [ComImport]
    public interface IComEventHandler
    {
        [DispId(1)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void OnPropertyChanged([MarshalAs(UnmanagedType.BStr), In] string propertyName);
    }

    [TypeLibType(16)]
    [ComEventInterface(typeof(IComEventHandler), typeof(SimpleObjectEventProvider))]
    [ComVisible(false)]
    public interface IComEvents
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void add_OnPropertyChanged([MarshalAs(UnmanagedType.BStr), In] ComEventHandler handler);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void remove_OnPropertyChanged([MarshalAs(UnmanagedType.BStr), In] ComEventHandler handler);
    }

    private void Init()
    {
        Guid riid = new ("71a4d526-4fad-4d4b-8a6e-78afcabd7f63");
        _connectionPointContainer.FindConnectionPoint(ref riid, out IConnectionPoint? connectionPoint);
        _connectionPoint = connectionPoint;
        _sinkHelpers = new List<SinkHelper>();
    }

    public void add_OnPropertyChanged([In] ComEventHandler handler)
    {
        lock (_lock)
        {
            if (_connectionPoint == null)
            {
                Init();
            }

            SinkHelper sink = new ();
            _connectionPoint!.Advise((object)sink, out int cookie);
            sink.Cookie = cookie;
            sink.Delegate = handler;
            _sinkHelpers!.Add(sink);
        }
    }

    /// <inheritdoc />
    public void remove_OnPropertyChanged([In] ComEventHandler handler)
    {
        lock (_lock)
        {
            if (_sinkHelpers == null)
            {
                return;
            }

            int count = _sinkHelpers.Count;
            int index = 0;
            if (0 >= count)
            {
                return;
            }

            do
            {
                SinkHelper sinkHelper = _sinkHelpers[index];
                if (sinkHelper.Delegate != null && ((sinkHelper.Delegate.Equals((object)handler) ? 1 : 0) & (int)byte.MaxValue) != 0)
                {
                    _sinkHelpers.RemoveAt(index);
                    _connectionPoint!.Unadvise(sinkHelper.Cookie);
                    if (count <= 1)
                    {
                        _connectionPoint = null;
                        _sinkHelpers = null;
                    }
                    break;
                }
                else
                    ++index;
            }
            while (index < count);
        }
    }


    ~SimpleObjectEventProvider() => Dispose(false);

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        _ = disposing;

        try
        {
            lock (_lock)
            {
                if (_connectionPoint == null || _sinkHelpers == null)
                {
                    return;
                }

                int count = _sinkHelpers.Count;
                int index = 0;
                if (0 >= count)
                {
                    return;
                }

                do
                {
                    _connectionPoint.Unadvise(_sinkHelpers[index].Cookie);
                    ++index;
                } while (index < count);
            }
        }
        catch (Exception)
        {
            // ... ignore error ...
        }
    }
}
