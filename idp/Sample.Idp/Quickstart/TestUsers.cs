// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Test;

namespace Sample.Idp.Quickstart;

public class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = 69118,
                country = "Germany"
            };
                
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "818727",
                    Username = "user",
                    Password = "user",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "User Name"),
                        new Claim(JwtClaimTypes.GivenName, "User"),
                        new Claim(JwtClaimTypes.FamilyName, "Name"),
                        new Claim(JwtClaimTypes.Email, "user@example.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json),
                    }
                },
            };
        }
    }
}
