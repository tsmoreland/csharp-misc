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

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TSMoreland.Interop.EventProviderGenerator;

internal sealed class SyntaxContextReceiver : ISyntaxContextReceiver
{
    private const string GeneratorAttributeName = "TSMoreland.Interop.EventProviderGenerator.Abstractions.ComEventProviderAttribute";
    private readonly List<MethodItem> _methods = new();

    public List<string> Log { get; } = new();
    public IEnumerable<MethodItem> Methods => _methods.AsEnumerable();

    private void SafeVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        if (context.Node is not InterfaceDeclarationSyntax interfaceDeclarationSyntax)
        {
            return;
        }

        var testInterface = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(context.Node)!;
        string? @namespace = testInterface.ContainingNamespace.ToString();

        if (IsGlobalNamespace(@namespace))
        {
            Log.Add($"Ignoring global namespace {@namespace}");
            return;
        }

        string interfaceName = testInterface.Name;

        Log.Add("Namespace: " + @namespace);
        Log.Add("Class: " + interfaceName);

        if (!IsGeneratorAttributePresent(testInterface))
        {
            return;
        }

        Log.Add($"Found attribute on {@namespace}.{interfaceName}");

        ImmutableArray<ISymbol> members = testInterface.GetMembers();

        foreach (ISymbol member in members)
        {

            ImmutableArray<AttributeData> attributes = member.GetAttributes();

            Log.Add(member.GetType().FullName);

            Log.Add($"\tmember: {member.Name} {member.OriginalDefinition} {member.ToDisplayString()}");

            List<ParameterItem> parameters = new();
            string returnType = string.Empty;
            if (member is IMethodSymbol method)
            {
                foreach (IParameterSymbol parameter in method.Parameters)
                {
                    returnType = method.ReturnType.Name;

                    Log.Add($"\t\tmember: {parameter.Type.Name} {parameter.Name}");
                    List<AttributeItem> attributeItems = new();
                    foreach (AttributeData attribute in parameter.GetAttributes())
                    {
                        string? attributeArguments = attribute.ConstructorArguments.Any()
                            ? attribute.ConstructorArguments
                                .Select(a => $"({a.Type!.Name}){a.Value?.ToString() ?? string.Empty}")
                                .Aggregate((a, b) => $"{a}, {b}")
                            : null;
                        attributeItems.Add(new AttributeItem(attribute.AttributeClass!.Name, attributeArguments));

                        if (attribute.ConstructorArguments.Any())
                        {
                            string arguments = attribute.ConstructorArguments
                                .Select(a => $"({a.Type!.Name}){a.Value?.ToString() ?? string.Empty}")
                                .Aggregate((a, b) => $"{a}, {b}");

                            Log.Add($"\t\t\t{attribute.AttributeClass!.Name}({arguments})");
                        }
                        else
                        {
                            Log.Add($"\t\t\t{attribute.AttributeClass!.Name}");
                        }
                    }
                    parameters.Add(new ParameterItem(parameter.Name, parameter.Type.Name,
                        attributeItems.ToImmutableArray()));
                }

            }

            int dispId = 0;
            foreach (AttributeData attribute in attributes.Where(a => a.AttributeClass?.Name == "DispIdAttribute"))
            {
                TypedConstant firstArgument = attribute.ConstructorArguments.FirstOrDefault();
                Log.Add($"\t\t{attribute.AttributeClass!.Name} '{firstArgument.Value}'");
                if (firstArgument.Value is int value)
                {
                    dispId = value;
                }
            }

            _methods.Add(new MethodItem(dispId, member.Name, returnType, parameters.ToImmutableArray()));
        }
    }

    private bool IsGeneratorAttributePresent(INamedTypeSymbol @interface)
    {
        ImmutableArray<AttributeData> allAttributes = @interface.GetAttributes();
        Log.AddRange(allAttributes.Select(attribute => $"Found {attribute.AttributeClass!.ContainingNamespace}.{attribute.AttributeClass!.Name}, looking for {GeneratorAttributeName}"));
        AttributeData[] attributes = allAttributes.Where(a => $"{a.AttributeClass!.ContainingNamespace}.{a.AttributeClass!.Name}" == GeneratorAttributeName).ToArray();
        return attributes.Any();
    }
    private static bool IsGlobalNamespace(string @namepsace)
    {
        // bit of a simple check that could be improved
        return namepsace.Contains("<");
    }


    /// <inheritdoc />
    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        try
        {
            SafeVisitSyntaxNode(context);
        }
        catch (Exception)
        {
            // ...
        }
    }

}
