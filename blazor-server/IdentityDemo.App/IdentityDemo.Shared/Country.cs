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
using System.Linq;

namespace IdentityDemo.Shared
{
    public sealed class Country : Entity<long>
    {
        public Country(long id)
            :  base(id)
        {
        }

        private Country()
        {
            // ... needed for EntityFramework ...
        }

        public string Name { get; private set; } = string.Empty;
        public int? Populatation { get; private set; } = null;

        private List<Province> _provinces = new List<Province>();

        public IEnumerable<Province> Provinces
        {
            get => _provinces.AsEnumerable();
            private set => _provinces = value is List<Province> provinces
                ? provinces
                : value.ToList();
        }

        /// <summary>
        /// Adds <paramref name="province"/> as a province
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="province"/> is null
        /// </exception>
        public void AddProvince(Province province)
        {
            if (province == null!)
                throw new ArgumentNullException(nameof(province));

            if (_provinces.Any(p => p.Id == province.Id))
                throw new ArgumentException("Cannot add duplicate province", nameof(province));

            province.State = ObjectState.Added;
            _provinces.Add(province);
        }

        /// <summary>
        /// Removes <paramref name="province"/> from <see cref="Provinces"/>
        /// </summary>
        /// <param name="province">province to remove</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="province"/> is null
        /// </exception>
        public void RemoveProvince(Province province)
        {
            if (province == null!)
                throw new ArgumentNullException(nameof(province));
            RemoveProvince(province.Id);
        }

        /// <summary>
        /// Removes province matching <paramref name="provinceId"/> from <see cref="Provinces"/>
        /// </summary>
        /// <param name="provinceId">id of province to remove</param>
        public void RemoveProvince(long provinceId)
        {
            var province = Provinces.FirstOrDefault(p => p.Id == provinceId);
            if (province != null)
                province.State = ObjectState.Deleted;
        }

        public Country With(long? id = null, string? name = null, int? population = null, IEnumerable<Province>? provinces = null) =>
            new Country(id ?? Id) {Provinces = provinces ?? Provinces, Populatation  = population ?? Populatation};
    }
}
