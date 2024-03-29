﻿//
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
using TSMoreland.ObjectTracker.Data.Abstractions.Entities;
using TSMoreland.ObjectTracker.Data.Abstractions.ViewModels;

namespace TSMoreland.ObjectTracker.Data.Abstractions;

public interface IObjectRepository : IDisposable, IAsyncDisposable
{
    Task<ObjectEntity> Add(ObjectEntity entity, CancellationToken cancellationToken);
    Task<LogEntity> AddMessage(int id, LogEntity entity, CancellationToken cancellationToken);
    IAsyncEnumerable<IdNamePair> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken);
    IAsyncEnumerable<LogViewModel> GetLogsForObjectById(int id, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<ObjectViewModel?> GetById(int id, CancellationToken cancellationToken);
    Task Update(int id, ObjectEntity entity, CancellationToken cancellationToken);
    Task Delete(int id, CancellationToken cancellationToken);

    Task<int> Commit(CancellationToken cancellationToken);
}
