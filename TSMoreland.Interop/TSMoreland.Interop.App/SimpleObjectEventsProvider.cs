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

using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace TSMoreland.Interop.App;

public sealed class SimpleObjectEventsProvider : ISimpleObjectEventsEvent, IDisposable
{
    private readonly IConnectionPointContainer _connectionPointContainer;
    private ArrayList? _eventSinkHelpers;
    private IConnectionPoint? _connectionPoint;
    private readonly object _lock = new ();

    public SimpleObjectEventsProvider([In] object connectionPointContainer)
    {
        _connectionPointContainer = (IConnectionPointContainer)connectionPointContainer;
    }

    private void Init()
    {
        Guid riid = new Guid(new byte[16] { (byte)38, (byte)213, (byte)164, (byte)113, (byte)173, (byte)79, (byte)75, (byte)77, (byte)138, (byte)110, (byte)120, (byte)175, (byte)202, (byte)189, (byte)127, (byte)99 });
        _connectionPointContainer.FindConnectionPoint(ref riid, out IConnectionPoint? connectionPoint);
        _connectionPoint = connectionPoint;
        _eventSinkHelpers = new ArrayList();
    }

    /// <inheritdoc />
    public void add_OnPropertyChanged([In] SimpleObjectEventsPropertyChangedEventHandler handler)
    {
        lock (_lock)
        {
            if (_connectionPoint == null)
            {
                Init();
            }

            SimpleObjectEventsSinkHelper sink = new ();
            _connectionPoint!.Advise((object)sink, out int cookie);
            sink.Cookie = cookie;
            sink.OnPropertyChangedDelegate = handler;
            _eventSinkHelpers!.Add((object)sink);
        }
    }

    /// <inheritdoc />
    public void remove_OnPropertyChanged([In] SimpleObjectEventsPropertyChangedEventHandler handler)
    {
        bool lockTaken = false;

        lock (_lock)
        {
            Monitor.Enter((object)this, ref lockTaken);
            if (_eventSinkHelpers == null)
            {
                return;
            }

            int count = _eventSinkHelpers.Count;
            int index = 0;
            if (0 >= count)
            {
                return;
            }

            do
            {
                var sinkHelper = (SimpleObjectEventsSinkHelper)_eventSinkHelpers[index]!;
                if (sinkHelper.OnPropertyChangedDelegate != null && ((sinkHelper.OnPropertyChangedDelegate.Equals((object)handler) ? 1 : 0) & (int)byte.MaxValue) != 0)
                {
                    _eventSinkHelpers.RemoveAt(index);
                    _connectionPoint!.Unadvise(sinkHelper.Cookie);
                    if (count <= 1)
                    {
                        //Marshal.ReleaseComObject((object)this._connectionPoint);
                        _connectionPoint = null;
                        _eventSinkHelpers = null;
                    }
                    break;
                }
                else
                    ++index;
            }
            while (index < count);
        }
    }

    ~SimpleObjectEventsProvider() => Dispose(false);

    /// <inheritdoc />
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
                if (_connectionPoint == null || _eventSinkHelpers == null)
                {
                    return;
                }

                int count = _eventSinkHelpers.Count;
                int index = 0;
                if (0 >= count)
                {
                    return;
                }

                do
                {
                    _connectionPoint.Unadvise(((SimpleObjectEventsSinkHelper)_eventSinkHelpers[index]!).Cookie);
                    ++index;
                } while (index < count);
                //Marshal.ReleaseComObject((object)_connectionPoint);
            }
        }
        catch (Exception)
        {
            // ... ignore error ...
        }
    }

}
