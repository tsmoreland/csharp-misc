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
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityDemo.Api.Models;
using IdentityDemo.Shared;
using Microsoft.EntityFrameworkCore;

namespace IdentityDemo.Api.Data
{
    /// <summary>
    /// ToDo:
    /// this should be shifted out of Api and into IdentityDemo.Data along with the context,
    /// The interface ICountryRepository should moved to shared or better yet a Country Specific assembly
    /// containing the models and DTOs if it were to use event messaging to communicate with
    /// other domain contexts
    /// </summary>
    public class CountryRepository : IDisposable, IAsyncDisposable
    {
        private readonly CountryDbContext _context;

        /// <summary>
        /// Instantiates a new instance of the <see cref="CountryRepository"/> class
        /// </summary>
        /// <param name="context">Database context</param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="context"/> is null
        /// </exception>
        public CountryRepository(CountryDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IAsyncEnumerable<KeyValuePair<long, string>> GetAllCountryNameAndIds() =>
            _context
                .Countries
                .AsQueryable()
                .Select(c => new KeyValuePair<long, string>(c.Id, c.Name))
                .AsAsyncEnumerable();

        public async Task<Country?> GetCountryByIdAsync(long id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
            if (country == null!)
                return null;

            var provinces = await _context
                .Provinces
                .Where(p => p.CountryId == country.Id)
                .ToListAsync();
            return country.With(provinces: provinces);
        }

        public IAsyncEnumerable<string> GetAllProvinceNamesForCountry(Country country) =>
            _context.Provinces
                .Where(p => p.CountryId == country.Id)
                .Select(p => p.Name)
                .AsAsyncEnumerable();

        /// <summary>
        /// Updates <paramref name="country"/> saving any additional child objects
        /// </summary>
        /// <param name="country">country to update</param>
        /// <returns><see cref="Task"/></returns>
        public async Task UpdateAsync(Country country)
        {
            foreach (var province in country.Provinces)
            {
                var state = province.State.ToEntityState();
                if (state == EntityState.Unchanged || state == EntityState.Detached)
                    continue;
                _context.Entry(province).State = state;
            }

            await _context.SaveChangesAsync();
        }

        #region IDisposable

        ///<summary>Finalize</summary>
        ~CountryRepository() => Dispose(false);

        ///<summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ///<summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        ///<param name="disposing">if <c>true</c> then release managed resources in addition to unmanaged</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        #endregion
        #region IAsyncDisposable

        public ValueTask DisposeAsync() =>
            _context.DisposeAsync();

        #endregion


    }
}
