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
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityDomain.Infrastructure
{
    /// <summary>
    /// The following would need database specific logic if we were actually implementing it
    /// </summary>
    public sealed class CustomIdentityUserStore
        : UserStoreBase<
            IdentityUser,
            string,
            IdentityUserClaim<string>,
            IdentityUserLogin<string>,
            IdentityUserToken<string>>
    {

        public CustomIdentityUserStore(IdentityErrorDescriber errorDescriber)
            : base(errorDescriber)
        {
            
        }

        public override IQueryable<IdentityUser> Users => throw new NotImplementedException();

        public override Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task AddLoginAsync(IdentityUser user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<Claim>> GetClaimsAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        protected override Task AddUserTokenAsync(IdentityUserToken<string> token)
        {
            throw new NotImplementedException();
        }

        protected override Task<IdentityUserToken<string>> FindTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<IdentityUser> FindUserAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<IdentityUserLogin<string>> FindUserLoginAsync(string userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<IdentityUserLogin<string>> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task RemoveUserTokenAsync(IdentityUserToken<string> token)
        {
            throw new NotImplementedException();
        }
    }
}
