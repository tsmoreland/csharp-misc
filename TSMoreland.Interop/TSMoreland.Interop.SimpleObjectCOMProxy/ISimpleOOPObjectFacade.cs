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

namespace TSMoreland.Interop.SimpleObjectCOMProxy;

public interface ISimpleOopObjectFacade : IDisposable
{
    event OOPOnPropertyChangedHandler PropertyChanged;

    /// <summary>
    /// not part of the COM interface, just helpful for diagnosing issues even if they do end up
    /// being a typo
    /// </summary>
    public object Object { get; }

    string Name { get; }
    int Numeric { get; set; }
    Guid Id { get; }

    /// <summary>
    /// Description which comes from ISimpleObject2 
    /// </summary>
    string Description { get; }

    string ConvertToString(Guid input);
}
