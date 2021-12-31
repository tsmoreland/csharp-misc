#if !NET5_0_OR_GREATER

using System.ComponentModel;

// ReSharper disable once CheckNamespace -- specific namespace required, needed to suppres warning due to use of init in netstandard2.0
namespace System.Runtime.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
internal class IsExternalInit{}

#endif