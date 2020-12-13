// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel;
using IdentityServer4.Test;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace ProjectX.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids => new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource("country", new [] { "country" })
        };

        public static IEnumerable<ApiResource> Apis => new ApiResource[] 
        {
            new ApiResource("projectx.rest", "ProjectX Web Api", new [] { "country" })
        };
        
        public static IEnumerable<Client> Clients => new Client[] 
        {
            new Client
            {
                Enabled = true,
                ClientName = "Postman",
                ClientId = "projectx.postman",
                RequireClientSecret = false,
                AllowedScopes = { "openid", "profile", "email", "country", "projectx.rest" },
                RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                PostLogoutRedirectUris = { "https://www.getpostman.com/oauth2/signout" },
                AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                AllowOfflineAccess = true,
                ClientUri = null
            },
            new Client
            {
                Enabled = true,
                ClientName = "Blazor",
                ClientId = "projectx.blazor",
                RequireClientSecret = false,
                AllowedScopes = { "openid", "profile", "email", "country", "projectx.rest" },
                RedirectUris = { "https://localhost:1003/authentication/login-callback" },
                PostLogoutRedirectUris = { "https://localhost:1003/" },
                AllowedGrantTypes = { GrantType.AuthorizationCode },
                AllowOfflineAccess = true,
                ClientUri = null,
                AllowedCorsOrigins = { "https://localhost:1003" }
            }
        };

        public static List<TestUser> Users => new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1da1e4ee-fef9-44d1-99f0-9777ea9a9d66",
                Username = "Craig",
                Password = "Welcome123",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Craig Hanson"),
                    new Claim(JwtClaimTypes.GivenName, "Craig"),
                    new Claim(JwtClaimTypes.FamilyName, "Hanson"),
                    new Claim(JwtClaimTypes.Email, "craigahanson@gmail.com"),
                    new Claim("country", "UK"),
                }
            },
            new TestUser
            {
                SubjectId = "8a8b75c4-f657-4f8b-976e-a7549c54863a",
                Username = "Heather",
                Password = "Welcome123",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Heather Phillips"),
                    new Claim(JwtClaimTypes.GivenName, "Heather"),
                    new Claim(JwtClaimTypes.FamilyName, "Phillips"),
                    new Claim(JwtClaimTypes.Email, "hl_phillips@hotmail.co.uk"),
                    new Claim("country", "UK"),
                }
            }
        };
    }
}