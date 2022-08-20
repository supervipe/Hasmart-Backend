// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;
using System.Security.Claims;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("hasmartapi", "Web API HASmart")
            };

        // TODO Rever forma de incluir clientes e usuarios
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "admin",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("1&ffyBN70BRG".Sha256())
                    },
                    // scopes that client has access to
                    AllowedScopes = 
                    {
                        "hasmartapi" 
                    },
                    ClientClaimsPrefix = string.Empty, 
                    Claims = new Claim[] // Assign const roles 
                    {
                        new Claim(JwtClaimTypes.Role, "admin"),
                    }
                },
                new Client
                {
                    ClientId = "Pmenos",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("AIct8jEu&on".Sha256())
                    },
                    // scopes that client has access to
                    AllowedScopes = { "hasmartapi" },
                    Claims = new Claim[] // Assign const roles 
                    {
                        new Claim(JwtClaimTypes.Role, "user"),
                    }
                },
                new Client
                {
                    ClientId = "swagger-ui",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("RFtkd#arYSIl".Sha256())
                    },
                    // scopes that client has access to
                    AllowedScopes = { "hasmartapi" },
                    Claims = new Claim[] // Assign const roles 
                    {
                        new Claim(JwtClaimTypes.Role, "user"),
                    }
                },
                new Client
                {
                    ClientId = "ClinicFarma",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("@EXoHShqsHwT".Sha256())
                    },
                    // scopes that client has access to
                    AllowedScopes = { "hasmartapi" },
                    Claims = new Claim[] // Assign const roles 
                    {
                        new Claim(JwtClaimTypes.Role, "user"),
                    }
                }
            };
    }
}