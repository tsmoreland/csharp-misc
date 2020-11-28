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

namespace AddressBook.Data.Entities
{
    public sealed class ContactEntity : Entity<Guid>
    {
        /// <summary>
        /// Instantiates a new instance of the <see cref="ContactEntity"/> class.
        /// </summary>
        /// <param name="id">unique id of the contact</param>
        public ContactEntity(Guid id)
            : base(id)
        {

        }
        private ContactEntity()
            : base(Guid.NewGuid())
        {
        }

        public string CompleteName { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } 
        public string Surname { get; set; } = string.Empty;

        public string? HomeStreet { get; set; }
        public string? HomeCity { get; set; }
        public string? HomeProvince { get; set; }
        public string? HomeCountryOrRegion { get; set; }
        public string? HomePostCode { get; set; }

        public string? BusinessStreet { get; set; } 
        public string? BusinessCity { get; set; } 
        public string? BusinessProvince { get; set; }
        public string? BusinessCountryOrRegion { get; set; }
        public string? BusinessPostCode { get; set; }

        public string? OtherStreet { get; set; } 
        public string? OtherCity { get; set; } 
        public string? OtherProvince { get; set; }
        public string? OtherCountryOrRegion { get; set; }
        public string? OtherPostCode { get; set; }

        public string? HomeEmailName { get; set; } 
        public string? HomeEmailAddress { get; set; } 
        public string? BusinessEmailName { get; set; } 
        public string? BusinessEmailAddress { get; set; } 
    }
}
