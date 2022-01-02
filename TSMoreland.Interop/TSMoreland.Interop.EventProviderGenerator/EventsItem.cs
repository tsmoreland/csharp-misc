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

using System.Collections.Immutable;
using System.Text;

namespace TSMoreland.Interop.EventProviderGenerator;

internal record struct EventsItem(string InterfaceName, string Namespace, ImmutableArray<MethodItem> Methods)
{

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

    public string BuildEventProvider()
    {
        throw new NotImplementedException();
    }

    public string BuildEventSink()
    {
        StringBuilder builder = new();

        AppendUsings(builder);
        builder
            .AppendLine($"namespace {Namespace}")
            .AppendLine("{")
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable enable
");
    }

}
