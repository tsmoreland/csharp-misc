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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AddressBook.Core.Model
{
    public sealed class Contact
    {
        private readonly ConcurrentDictionary<AddressKey, PhysicalAddress> _physicalAddresses;
        private readonly ConcurrentDictionary<AddressKey, EmailAddress> _emailAddresses;

        public Contact()
            : this(Guid.NewGuid())
        {
        }

        public Contact(Guid id)
        {
            _physicalAddresses = new ConcurrentDictionary<AddressKey, PhysicalAddress>();
            _emailAddresses = new ConcurrentDictionary<AddressKey, EmailAddress>();
            Id = id;
        }

        internal Guid Id { get; } 

        public string CompleteName { get; init; } = string.Empty;
        public string GivenName { get; init; } = string.Empty;
        public string MiddleName { get; init; } = string.Empty;
        public string Surname { get; init; } = string.Empty;

        public IEnumerable<PhysicalAddress> PhysicalAddresses =>
            _physicalAddresses.Values.AsEnumerable();

        public IEnumerable<EmailAddress> EmailAddresses =>
            _emailAddresses.Values.AsEnumerable();

        /// <summary>
        /// Attempts to set address 
        /// </summary>
        /// <param name="key">key or type of the address to set</param>
        /// <param name="physicalAddress">the address value</param>
        /// <returns>
        /// returns true on sucess; otherwise, false
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// if address is null
        /// </exception>
        public bool TrySetPhysicalAddress(AddressKey key, PhysicalAddress physicalAddress)
        {
            if (physicalAddress is null)
                throw new ArgumentNullException(nameof(physicalAddress));

            try
            {
                _physicalAddresses.AddOrUpdate(key, physicalAddress,
                    (_, _) => physicalAddress);
                return true;
            }
            catch (OverflowException)
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to set address 
        /// </summary>
        /// <param name="key">key or type of the address to set</param>
        /// <param name="address">the address value</param>
        /// <returns>
        /// returns true on sucess; otherwise, false
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// if address is null
        /// </exception>
        public bool TrySetEmailAddress(AddressKey key, EmailAddress address)
        {
            if (address is null)
                throw new ArgumentNullException(nameof(address));

            try
            {
                _emailAddresses.AddOrUpdate(key, address,
                    (_, _) => address);
                return true;
            }
            catch (OverflowException)
            {
                return false;
            }
        }
    }
}
