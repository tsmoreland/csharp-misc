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
namespace TSMoreland.ObjectTracker.Data.Abstractions.Entities;

public sealed class ObjectEntity
{
    public ObjectEntity(int id, string name, int progress, Address address, DateTime lastModified, List<LogEntity>? logs)
    {
        Id = id;
        Name = name;
        Progress = progress;
        Address = address;
        Logs = logs?.ToList() ?? new List<LogEntity>();
        LastModified = lastModified;
    }
    private ObjectEntity()
    {
    }

    public int Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public int Progress { get; set; } = 0;
    public Address Address { get; private set; } = Address.None;
    public DateTime LastModified { get; set; } = DateTime.MinValue;

    public List<LogEntity> Logs { get; } = new();

}
