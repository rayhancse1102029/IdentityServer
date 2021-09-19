using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentiyServer4.Models
{
    public class Config
    {
        public static IEnumerable<Client> GetClients()
        {

            return new List<Client>
            {
                //Client-Credential based grant type
                new Client
                {
                    ClientId = "rayhancse15@gmail.com",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("pass123#".Sha256())
                    },
                    AllowedScopes = { "identityServer4Scope" }
                },
                new Client
                {
                    ClientId = "rayhan",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("pass123#".Sha256())
                    },
                    AllowedScopes = { "identityServer4Scope" }
                },
                new Client
                {
                    ClientId = "iBOS",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("pass123#".Sha256())
                    },
                    AllowedScopes = { "identityServer4Scope" }
                },
                //Resource Owner Password grant type
                new Client
                {
                    ClientId = "ro.iBOS",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("pass123#".Sha256())
                    },
                    AllowedScopes = { "identityServer4Scope" }
                },

                //Implicit Flow Grant Type
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    // OLD  AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc"},
                    ClientSecrets =
                    {
                        new Secret("pass123#".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                   
                },

                //Swagger Client
                new Client
                {
                    ClientId = "swagapi",
                    ClientName = "Swagger API",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { "http://localhost:59337/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { "http://localhost:59337/swagger" },

                    
                    ClientSecrets =
                    {
                        new Secret("pass123#".Sha256())
                    },
                }
            };
        }
        public static IEnumerable<ApiScope> GetScopes()
        {
            return new[]
            {
                new ApiScope("identityServer4Scope","identityServer4 Scope")
            };
        }
        public static IEnumerable<ApiResource> GetAllApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("identityServer4Scope", "identityServer4 Scope")
            };
        }
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                //new TestUser
                //{
                //    SubjectId = "1",
                //    Username = "abu",
                //    Password = "password",
                //    Claims = new List<Claim>
                //    {
                //        new Claim(JwtClaimTypes.GivenName, "abu"),
                //        new Claim(JwtClaimTypes.FamilyName, "my family")
                //    }
                    
                //},
                //new TestUser
                //{
                //    SubjectId = "2",
                //    Username = "rayhan",
                //    Password = "password"
                //}
            };
        }
       

        
    }
}
