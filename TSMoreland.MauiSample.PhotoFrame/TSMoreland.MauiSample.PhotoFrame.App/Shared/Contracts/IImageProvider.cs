//
// Copyright (c) 2022 Terry Moreland
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

namespace TSMoreland.MauiSample.PhotoFrame.App.Shared.Contracts;

public interface IImageProvider
{
    /// <summary>
    /// returns <see langword="true"/> if images were loaded with shuffle set
    /// </summary>
    bool Shuffle { get; }

    /// <summary>
    /// returns <see langword="true"/> if images were loaded with repeat set
    /// </summary>
    bool Repeat { get; }

    /// <summary>
    /// Returns <see langword="true"/> if provider has at least one more image
    /// </summary>
    bool HasNext { get; }

    /// <summary>
    /// Returns the next image
    /// </summary>
    Task<ImageSource> NextAsync(CancellationToken cancellationToken);

    /// <summary>
    /// initialzies the provider with the provided path and settings
    /// </summary>
    Task InitializeFromFolder(string path, bool shuffle, bool repeat, CancellationToken cancellationToken);
}
