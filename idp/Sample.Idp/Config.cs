// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace Sample.Idp
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources => 
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("custom", new [] { "custom" })
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("identitydemoapi", "IdentityDemo API", new [] { "custom" }),
            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "identitydemoapp",
                    ClientName = "IdentityDemo",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                    RequirePkce = true,
                    RedirectUris = { "https://localhost:44376/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44376/signout-oidc" },
                    AllowedScopes = { "openid", "profile", "email", "identitydemoapi" }
                },
            };
    }
}
