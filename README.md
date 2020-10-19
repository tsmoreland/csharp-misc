![Trunk CI Build](https://github.com/tsmoreland/Util/workflows/Trunk%20CI%20Build/badge.svg)

# Moreland.CSharp.Util

Utilty object library provided generic support types such as results, optionals, etc.. for use in other software packages

## Result Types

3 result types: ```QueryResult<T>```, ```CommandResult```, and ```CommandAndQueryResult<T>```; ```CommandAndQueryResult<T>``` violates CQS but is provided because while command query seperation should be strived for it's not always possible with given code.

## Support Classes

- ```Maybe<T>``` a C# equivalent of java.util.Optional<T>
- NumericParser, class providing MaybeParse...() methods with the same similar 
- HashCodeBuilder; builder which creates a HashCode for multiple values, similar to ```HasCode.Combine<T1>(T1 value)``` available in newer versions of .NET (dotnet standard 2.1+)

## Acknowledgements

Either class initially based primarily on examples given during [Marking Functional C#](https://app.pluralsight.com/library/courses/making-functional-csharp)
by [Zoran Horvat](http://twitter.com/zoranh75)
