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

using AddressBook.Core.Interfaces;
using AddressBook.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace AddressBook.Data.Repositories
{
    public sealed class ContactRepository : IContactRepository
    {
        private readonly ContactDatabaseContext _contactDatabaseContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Instantiates a new instance of the <see cref="ContactDatabaseContext"/> class.
        /// </summary>
        /// <param name="contactDatabaseContext">database context</param>
        /// <param name="mapper"></param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="contactDatabaseContext"/> is null
        /// </exception>
        public ContactRepository(ContactDatabaseContext contactDatabaseContext, IMapper mapper)
        {
            _contactDatabaseContext = contactDatabaseContext ?? throw new ArgumentNullException(nameof(contactDatabaseContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Contact? FindByCompleteName(string completeName)
        {
            var entity = _contactDatabaseContext.Contacts.SingleOrDefault(c => c.CompleteName == completeName);
            return entity is null 
                ? null 
                : _mapper.Map<Contact>(entity);
        }
        public IEnumerable<Contact> GetAll()
        {
            return Array.Empty<Contact>();
        }
    }
}
