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
using System.Collections.Concurrent;
using System.Linq;
using AddressBook.Core.Interfaces;

namespace AddressBook.Core.Model
{
    public sealed class Contact : IEntityBacked<Guid>
    {
        private readonly ConcurrentDictionary<AddressKey, PhysicalAddress> _physicalAddresses;
        private readonly List<EmailAddress> _emailAddresses;
        private string _givenName = string.Empty;
        private string _surname = string.Empty;

        public Contact()
            : this(Guid.NewGuid())
        {
        }

        public Contact(Guid id)
        {
            _physicalAddresses = new();
            _emailAddresses = new();
            Id = id;
        }

        /// <inheritdoc/>
        public Guid Id { get; }

        public string CompleteName { get; private set; } = string.Empty;
        public string GivenName 
        { 
            get => _givenName;
            set
            {
                if (_givenName == value)
                    return;
                var originalValue = _givenName;
                _givenName = value;
                if (CompleteName is not {Length: > 0} || CompleteName == originalValue || CompleteName == Surname)
                    UpdateCompleteName();
            }
        } 
        public string MiddleName { get; private set; } = string.Empty;
        public string Surname
        {
            get => _surname;
            set
            {
                if (_surname == value)
                    return;
                var originalValue = _surname;
                _surname = value;
                if (CompleteName is not {Length: > 0} || CompleteName == GivenName || CompleteName == originalValue)
                    UpdateCompleteName();
            }
        }

        public IEnumerable<PhysicalAddress> PhysicalAddresses =>
            _physicalAddresses.Values.AsEnumerable();

        public IEnumerable<EmailAddress> EmailAddresses =>
            _emailAddresses.AsEnumerable();

        public bool TryAddOrUpdateAddress(AddressKey key, PhysicalAddress address)
        {
            try
            {
                _physicalAddresses.AddOrUpdate(key, address, (_, _) => address);
                return true;
            }
            catch (OverflowException)
            {
                return false;
            }
        }

        public bool TryAddAddress(EmailAddress address)
        {
            if (_emailAddresses.Contains(address))
                return false;

            _emailAddresses.Add(address);
            return true;
        }

        public bool TryRemoveAddress(AddressKey key) =>
            _physicalAddresses.TryRemove(key, out _);

        public bool TryRemoveAddress(EmailAddress address)
        {
            if (!_emailAddresses.Contains(address))
                return false;

            _emailAddresses.Remove(address);
            return true;
        }

        private void UpdateCompleteName()
        {
            var hasGivenName = GivenName is {Length: > 0};
            var hasSurname = Surname is {Length: > 0};

            if (hasGivenName && hasSurname)
                CompleteName = $"{GivenName} {Surname}";
            else if (hasGivenName)
                CompleteName = GivenName;
            else if (hasSurname)
                CompleteName = Surname;
        }
    }
}
