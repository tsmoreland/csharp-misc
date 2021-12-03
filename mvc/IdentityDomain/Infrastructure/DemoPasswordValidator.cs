﻿//
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
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IdentityDomain.Infrastructure
{
    public sealed class DemoPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        public DemoPasswordValidator()
        {
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            var errors = new List<IdentityError>();
            var username = await manager.GetUserNameAsync(user) ?? string.Empty;
            if (password.Contains(username, StringComparison.InvariantCultureIgnoreCase))
                errors.Add(Error("Password cannot contain username"));

            var reverseUsername = new string(username.ToCharArray().Reverse().ToArray());
            if (password.Contains(reverseUsername, StringComparison.InvariantCultureIgnoreCase))
                errors.Add(Error("Password cannot contain reversed username"));

            return !errors.Any()
                ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray());

            static IdentityError Error(string description) => new IdentityError {Description = description};
        }
    }
}
