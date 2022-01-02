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

internal record struct MethodItem(
    int DispId,
    string Name,
    string Type,
    ImmutableArray<ParameterItem> Parameters)
{

    public string DelegateName = $"{Name}Handler";


    public void AppendDelegateImplementation(StringBuilder builder)
    {
        builder.AppendLine($"        public {DelegateName}? {DelegateName}Delegate {{ get; set; }}");
    }

    public void AppendEventBridgeImplementation(StringBuilder builder)
    {
        string arguments = Parameters.Any()
            ? Parameters
                .Select(p => p.Name)
                .Aggregate((a, b) => $"{a}, {b}")
            : string.Empty;

        builder
            .AppendLine($"        public void {Name}({FormatParameters()}) =>")
            .AppendLine($"            {DelegateName}Delegate?.Invoke({arguments});");

    }

    public string ParametersAsArgumentsForCaller =>
        $"({FormatParametersAsArgumentsForCaller()})";

    private string FormatParametersAsArgumentsForCaller() => Parameters.Any()
        ? Parameters
            .Select(p => p.Name)
            .Aggregate((a, b) => $"{a}, {b}")
        : string.Empty;


    public string Delegate => new StringBuilder()
        .AppendLine("    [ComVisible(false)]")
        .AppendLine($"    public delegate void {DelegateName}({FormatParameters()});")
        .ToString();


    private string FormatParameters() => Parameters.Any()
        ? Parameters
            .Select(p => p.ToString())
            .Aggregate((a, b) => $"{a}, {b}")
        : string.Empty;

    /// <inheritdoc />
    public override string ToString() => $"{Name}({FormatParameters()});";
}
