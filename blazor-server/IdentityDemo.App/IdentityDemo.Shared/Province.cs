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

namespace IdentityDemo.Shared
{
    public sealed class Province : Entity<long>
    {
        public Province(long id)
            : base(id)
        {

        }

        private Province()
        {
            // ... needed for entity framework ...
        }

        public static Province Create(long countryId, string name, int? population) =>
            new Province {Name = name, Populatation = population, CountryId = countryId};

        /// <summary>
        /// Internal tracking state used to handle adds to parent object
        /// </summary>
        public ObjectState State { get; set; }

        public string Name { get; private set; } = string.Empty;
        public int? Populatation { get; private set; } = null;
        public long CountryId { get; private set; }


        public void SetNameIfEmpty(string name)
        {
            if (string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(name))
                Name = name;
        }

        /// <summary>
        /// Sets the population value
        /// </summary>
        /// <param name="population">new population to use</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// if <paramref name="population"/> is negative
        /// </exception>
        public void SetPopulation(int population)
        {
            if (population < 0)
                throw new ArgumentOutOfRangeException(nameof(population), $"{nameof(population)} must be greater than or equal to 0");
            Populatation = population;
        }

    }
}
