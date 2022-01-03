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

using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace TSMoreland.Interop.EventProviderGenerator;

[Generator]
internal class Generator : ISourceGenerator
{

    /// <inheritdoc />
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxContextReceiver());
    }

    /// <inheritdoc />
    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is not SyntaxContextReceiver receiver)
        {
            context.AddSource("GeneratorLogs", SourceText.From($@"/*{ context.SyntaxContextReceiver?.GetType().FullName ?? "Unknown"}*/", Encoding.UTF8));
            return;
        }

        receiver.Log.Add("===================================");
        foreach (EventsItem @event in receiver.Events)
        {
            receiver.Log.Add($"{@event.Namespace}.{@event.InterfaceName}");
            foreach (MethodItem method in @event.Methods)
            {
                receiver.Log.Add("\t" + method);
            }

            receiver.Log.Add("============= delegates ===================");
            receiver.Log.Add(@event.BuildEventDelegates());

            receiver.Log.Add("============= bridge ===================");
            receiver.Log.Add(@event.BuildEventsBridgeInterface());

            receiver.Log.Add("============= sink helper ===================");
            receiver.Log.Add(@event.BuildEventSink());

            receiver.Log.Add("============= event provider ===================");
            receiver.Log.Add(@event.BuildEventProvider());

            context.AddSource(@event.DelegatesFilename, SourceText.From(@event.BuildEventDelegates(), Encoding.UTF8));
            context.AddSource(@event.BridgeInterfaceFilename, SourceText.From(@event.BuildEventsBridgeInterface(), Encoding.UTF8));
            context.AddSource(@event.SinkHelperFilename, SourceText.From(@event.BuildEventSink(), Encoding.UTF8));
            context.AddSource(@event.EventProviderFilename, SourceText.From(@event.BuildEventProvider(), Encoding.UTF8));
        }


        context.AddSource("GeneratorLogs", SourceText.From($@"/*{ Environment.NewLine + string.Join(Environment.NewLine, receiver.Log) + Environment.NewLine}*/", Encoding.UTF8));
    }

}
