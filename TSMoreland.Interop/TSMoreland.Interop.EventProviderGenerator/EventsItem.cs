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

using System.Collections.Immutable;
using System.Text;

namespace TSMoreland.Interop.EventProviderGenerator;

internal record struct EventsItem(
    string InterfaceName,
    string InterfaceId,
    string Namespace,
    ImmutableArray<MethodItem> Methods)
{

    public string DelegatesFilename =>
        $"{InterfaceName}Delegates.cs";

    public string BuildEventDelegates()
    {
        StringBuilder builder = new();
        AppendUsings(builder);

        builder
            .AppendLine($@"namespace {Namespace}")
            .AppendLine("{");

        foreach (MethodItem method in Methods)
        {
            builder.AppendLine($"{method.Delegate}");
        }

        builder.AppendLine("}");

        return builder.ToString();
    }

    public string BridgeInterfaceFilename =>
        $"{BridgeName}.cs";

    public string BuildEventsBridgeInterface()
    {
        StringBuilder builder = new();

        AppendUsings(builder);
        builder.AppendLine($@"
namespace {Namespace}
{{
    [TypeLibType(16)]
    [ComEventInterface(typeof({InterfaceName}), typeof({EventProviderName}))]
    [ComVisible(false)]
    public interface {BridgeName}
    {{");
        foreach (MethodItem method in Methods)
        {
            builder.AppendLine($@"
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void add_{method.Name}([In] {method.DelegateName} handler);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void remove_{method.Name}([In] {method.DelegateName} handler);");
        }

        builder
            .AppendLine("    }")
            .AppendLine("}");


        return builder.ToString();
    }

    public string EventProviderFilename =>
        $"{EventProviderName}.cs";

    public string BuildEventProvider()
    {
        StringBuilder builder = new();

        AppendUsings(builder);

        builder.AppendLine($@"namespace {Namespace}
{{
    public sealed class {EventProviderName} : {BridgeName}, IDisposable
    {{
        private readonly IConnectionPointContainer _connectionPointContainer;
        private List<{SinkHelperName}>? _sinkHelpers;
        private IConnectionPoint? _connectionPoint;
        private readonly object _lock = new();

        public {EventProviderName}(object? @object)
        {{
            if (@object is not IConnectionPointContainer connectionPointContainer)
            {{
                throw new ArgumentException(""Argument is not a connection point container"", nameof(@object));
            }}
            _connectionPointContainer = connectionPointContainer;
        }}

        private void Init()
        {{
            Guid riid = new (""{InterfaceId}"");
            _connectionPointContainer.FindConnectionPoint(ref riid, out IConnectionPoint? connectionPoint);
            _connectionPoint = connectionPoint;
            _sinkHelpers = new List<{SinkHelperName}>();
        }}

        ~{EventProviderName}() => Dispose(false);
        public void Dispose()
        {{
            Dispose(true);
            GC.SuppressFinalize(this);
        }}
        private void Dispose(bool disposing)
        {{
            _ = disposing;
            try
            {{
                lock (_lock)
                {{
                    if (_connectionPoint == null || _sinkHelpers == null)
                    {{
                        return;
                    }}

                    int count = _sinkHelpers.Count;
                    int index = 0;
                    if (0 >= count)
                    {{
                        return;
                    }}

                    do
                    {{
                        _connectionPoint.Unadvise(_sinkHelpers[index].Cookie);
                        ++index;
                    }} while (index < count);
                }}
            }}
            catch (Exception ex)
            {{
                System.Diagnostics.Trace
                    .TraceError($""Exception occurred disposing {EventProviderName}: {{ex.GetType()}} {{ex.Message}}"");
            }}
        }}
");

        foreach (MethodItem method in Methods)
        {
            AppendEventBridgeMethodImplementation(builder, method);
        }

        builder
            .AppendLine("   }")
            .AppendLine("}");

        return builder.ToString();
    }
    private void AppendEventBridgeMethodImplementation(StringBuilder builder, MethodItem method)
    {
        builder.Append($@"        public void add_{method.Name}([In] {method.DelegateName} handler)
        {{
            lock (_lock)
            {{
                if (_connectionPoint == null)
                {{
                    Init();
                }}
                {SinkHelperName} sink = new ();
                _connectionPoint!.Advise((object)sink, out int cookie);
                sink.Cookie = cookie;
                sink.{method.DelegatePropertyName} = handler;
                _sinkHelpers!.Add(sink);
            }}
        }}
        public void remove_{method.Name}([In] {method.DelegateName} handler)
        {{
            lock (_lock)
            {{
                if (_sinkHelpers == null)
                {{
                    return;
                }}

                int count = _sinkHelpers.Count;
                int index = 0;
                if (0 >= count)
                {{
                    return;
                }}

                do
                {{
                    {SinkHelperName} sinkHelper = _sinkHelpers[index];
                    if (sinkHelper.{method.DelegatePropertyName} != null &&
                        ((sinkHelper.{method.DelegatePropertyName}.Equals((object)handler) ? 1 : 0) & (int)byte.MaxValue) != 0)
                    {{
                        _sinkHelpers.RemoveAt(index);
                        _connectionPoint!.Unadvise(sinkHelper.Cookie);
                        if (count <= 1)
                        {{
                            _connectionPoint = null;
                            _sinkHelpers = null;
                        }}
                        break;
                    }}
                    else
                    {{
                        ++index;
                    }}
                }}
                while (index < count);
            }}
        }}
");

    }

    public string SinkHelperFilename =>
        $"{SinkHelperName}.cs";

    public string BuildEventSink()
    {
        StringBuilder builder = new();

        AppendUsings(builder);
        builder
            .AppendLine($"namespace {Namespace}")
            .AppendLine("{")
            .AppendLine("    [ClassInterface(ClassInterfaceType.None)]")
            .AppendLine("    [TypeLibType(TypeLibTypeFlags.FHidden)]")
            .AppendLine($"    internal sealed class {SinkHelperName} : {InterfaceName}")
            .AppendLine("    {")
            .AppendLine("        public int Cookie { get; set; }")
            .AppendLine();
        foreach (MethodItem method in Methods)
        {
            method.AppendDelegateImplementation(builder);
            method.AppendEventBridgeImplementation(builder);
        }

        builder
            .AppendLine("   }")
            .AppendLine("}");


        return builder.ToString();
    }

    private string BridgeName => $"{InterfaceName}Bridge";
    private string EventProviderName =>
         InterfaceName.TrimStart('I') + "Provider";

    private string SinkHelperName =>
         InterfaceName.TrimStart('I') + "SinkHelper";

    private static void AppendUsings(StringBuilder builder)
    {
        builder.AppendLine(@"
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable enable
");
    }

}
