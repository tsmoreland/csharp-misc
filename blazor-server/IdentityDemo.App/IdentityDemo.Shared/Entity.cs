//
// Copyright (c) 2020 Terry Moreland
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

namespace IdentityDemo.Shared
{
    public abstract class Entity<TId> : IEqualityComparer<Entity<TId>>
        where TId : struct
    {
        /// <summary>
        /// instantiates a new instance of the <see cref="Entity{TId}"/> class.
        /// </summary>
        /// <param name="id">unique identifier for the entity</param>
        /// <exception cref="ArgumentException">
        /// if <paramref name="id"/> is the default for <typeparamref name="TId"/>
        /// </exception>
        protected Entity(TId id)
        {
            if (id.Equals(default(TId)))
                throw new ArgumentException("Invalid Id Value", nameof(id));
            Id = id;
        }

        protected Entity()
        {
            // ... required by Entity Framework ...
        }

        public TId Id { get; protected set; }

        /// <inheritdoc />
        public override bool Equals(object obj) =>
            obj is Entity<TId> entity && Equals(this, entity);

        /// <inheritdoc/>
        public bool Equals(Entity<TId> x, Entity<TId> y) =>
            ReferenceEquals(x, y) || 
            x != null! && x.Id.Equals(y.Id);

        /// <inheritdoc />
        public override int GetHashCode() => 
            GetHashCode(this);

        /// <inheritdoc />
        public int GetHashCode(Entity<TId> obj)
        {
            if (obj == null!)
                throw new ArgumentNullException(nameof(obj));
            return obj.Id.GetHashCode();
        }
    }
}
