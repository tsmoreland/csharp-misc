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

namespace AddressBook.Core.Entities
{
    public class ContactEntity : Entity<int>
    {
        public ContactEntity(int id)
            : base(id)
        {
            
        }
        private ContactEntity()
            : base(0)
        {
        }

        public string CompleteName { get; init; } = string.Empty;
        public string GivenName { get; init; } = string.Empty;
        public string? MiddleName { get; init; } 
        public string Surname { get; init; } = string.Empty;

        public string? HomeStreet { get; init; }
        public string? HomeCity { get; init; }
        public string? HomeProvince { get; init; }
        public string? HomeCountryOrRegion { get; init; }
        public string? HomePostCode { get; init; }

        public string? BusinessStreet { get; init; } 
        public string? BusinessCity { get; init; } 
        public string? BusinessProvince { get; init; }
        public string? BusinessCountryOrRegion { get; init; }
        public string? BusinessPostCode { get; init; }

        public string? OtherStreet { get; init; } 
        public string? OtherCity { get; init; } 
        public string? OtherProvince { get; init; }
        public string? OtherCountryOrRegion { get; init; }
        public string? OtherPostCode { get; init; }
    }
}
