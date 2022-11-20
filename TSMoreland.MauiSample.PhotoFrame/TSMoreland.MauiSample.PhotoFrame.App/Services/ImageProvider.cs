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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSMoreland.MauiSample.PhotoFrame.App.Shared.Contracts;

namespace TSMoreland.MauiSample.PhotoFrame.App.Services;

public sealed class ImageProvider : IImageProvider
{
    private readonly List<string> _files = new();
    private readonly object _filesLock = new();
    private readonly Random _random = new();
    private int _index = -1;


    /// <inheritdoc />
    public bool Shuffle { get; private set; }

    /// <inheritdoc />
    public bool Repeat { get; private set; }

    /// <inheritdoc />
    public bool HasNext
    {
        get
        {
            lock (_filesLock)
            {
                return _files.Any();
            }
        }
    }

    /// <inheritdoc />
    public Task<ImageSource> NextAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Task InitializeFromFolder(string path, bool shuffle, bool repeat, CancellationToken cancellationToken)
    {
        lock (_filesLock)
        {
            Shuffle = shuffle;
            Repeat = repeat;

            List<string> files = new();
            files.AddRange(Directory.GetFiles(path, "*.jpg"));
            files.AddRange(Directory.GetFiles(path, "*.png"));

            _files.Clear();
            if (Shuffle)
            {
                _files.AddRange(files);
            }
            else
            {
                while (files.Any())
                {
                    int index = _random.Next(0, files.Count);
                    _files.Add(files[index]);
                    files.RemoveAt(index);
                }
            }

            _index = 0;
        }

        return LoadImageAsync();
    }

    private async Task LoadImageAsync()
    {
        try
        {


        }
        catch (Exception)
        {
        }

        await Task.CompletedTask;
    }
}
