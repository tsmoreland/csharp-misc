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
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;
using TSMoreland.MauiSample.PhotoFrame.App.Shared.Contracts;

namespace TSMoreland.MauiSample.PhotoFrame.App.Services;

public sealed class ImageProvider : IImageProvider
{
    private readonly List<string> _files = new();
    private readonly object _filesLock = new();
    private readonly Random _random = new();
    private int _index = -1;
    private byte[]? _buffer = null;


    /// <inheritdoc />
    public bool Shuffle { get; private set; }

    /// <inheritdoc />
    public bool Repeat { get; private set; }

    /// <inheritdoc />
    public Task<ImageSource?> NextAsync(ImageSource? current, CancellationToken cancellationToken)
    {
        _index++;
        if (_index >= _files.Count)
        {
            if (!Repeat)
            {
                return Task.FromResult<ImageSource?>(null);
            }
        }

        string file = _files[_index];
        return GetImagesSourceFromFile(file);
    }

    private async Task<ImageSource?> GetImagesSourceFromFile(string filename)
    {
        long size = new FileInfo(filename).Length;
        _buffer = ArrayPool<byte>.Shared.Rent((int)size);

        await using (FileStream fs = new(_files[_index], FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite))
        {
            Memory<byte> memory = new(_buffer);
            size = await fs.ReadAsync(memory);
            fs.Close();
        }
        MemoryStream ms = new(_buffer, 0, (int)size);
        ImageSource source = ImageSource.FromStream(() => ms);
        return source;
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

            _index = -1;
        }

        return Task.CompletedTask;
    }
}
