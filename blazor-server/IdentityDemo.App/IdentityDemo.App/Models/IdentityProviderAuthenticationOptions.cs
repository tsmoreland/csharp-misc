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

namespace IdentityDemo.App.Models
{
    /// <summary>
    /// Extended configuration options used to setup IdP separated out
    /// as consumers of the IdP within the code base don't need this detail
    /// </summary>
    public sealed class IdentityProviderAuthenticationOptions
    {
        /// <summary>
        /// Expected name for use in appsettings.json
        /// </summary>
        public static string SectionName { get; }  = "IdentityProviderAuthentication";

        public string ResponseType { get; set; } = string.Empty;
        public string NameClaimType { get; set; } = "given_name";
        public bool SaveTokens { get; set; } = true;
        public bool GetClaimsFromUserInfoEndPoint { get; set; } = true;

        public List<string> ApiScopes { get; set; } = new List<string>();
    }
}
