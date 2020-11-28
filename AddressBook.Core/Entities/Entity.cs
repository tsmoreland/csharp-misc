//
// Copyright © 2020 Terry Moreland
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
using System.Diagnostics.CodeAnalysis;

namespace AddressBook.Core.Entities
{
    [SuppressMessage("Major Code Smell", "S4035:Classes implementing \"IEquatable<T>\" should be sealed", Justification = "Equatability is intentionally limited to identifier which is contained within this class")]
    public abstract class Entity<TIdentifier>
        : IEquatable<Entity<TIdentifier>>
        , IEqualityComparer<Entity<TIdentifier>>
        where TIdentifier : struct
    {
        protected Entity(TIdentifier id)
        {
            Id = id;
        }
        private Entity()
        {
            // .. needed for enttiy framework ..
        }

        public TIdentifier Id { get; protected set; } = default!;

        /// <inheritdoc />
        public override int GetHashCode() =>
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            EqualityComparer<TIdentifier>.Default.GetHashCode(Id);

        /// <inheritdoc />
        /// <remarks>
        /// the following assumes all inherting class equality is solely
        /// based on identifier
        /// </remarks>
        public override bool Equals(object? obj) =>
            obj is Entity<TIdentifier> entity && Equals(entity);

        /// <inheritdoc />
        public bool Equals(Entity<TIdentifier>? other) =>
            other is not null &&
            GetType() == other.GetType() &&
            EqualityComparer<TIdentifier>.Default.Equals(Id, other.Id);

        /// <inheritdoc/>
        public bool Equals(Entity<TIdentifier>? x, Entity<TIdentifier>? y) =>
            ReferenceEquals(x, y) || x is not null && x.Equals(y);

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] Entity<TIdentifier> obj) =>
            obj.GetHashCode();

    }
}
