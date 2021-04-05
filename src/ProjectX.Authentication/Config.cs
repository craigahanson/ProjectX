// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Duende.IdentityServer.Models;
using System.Collections.Generic;
using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;
using IdentityModel;

namespace ProjectX.Authentication
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("projectx.rest", "ProjectX Rest")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "testclient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        "projectx.rest"
                    }
                },
                new Client
                {
                    ClientId = "projectx.postman",
                    RequireClientSecret = false,
                    AllowedScopes = { "openid", "profile", "email", "country", "projectx.rest" },
                    RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
                    PostLogoutRedirectUris = { "https://www.getpostman.com/oauth2/signout" },
                    AllowedGrantTypes = { GrantType.ResourceOwnerPassword }
                },
                new Client
                {
                    ClientId = "projectx.blazor",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:1003/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:1003" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile, 
                        IdentityServerConstants.StandardScopes.Email, 
                        "country", 
                        "projectx.rest"
                    },
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
                    new Claim("country", "UK")
                }
            }
        };
    }
}