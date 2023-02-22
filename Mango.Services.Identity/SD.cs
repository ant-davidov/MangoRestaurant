using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using System.Security.Cryptography;

namespace Mango.Services.Identity
{
    public static class SD
    {
        public static string Admin = "Admin";
        public static string Customer = "Customer";
        public static IEnumerable<IdentityResource> IdentityResources
                => new List<IdentityResource>
                {
                   new IdentityResources.OpenId(),
                   new IdentityResources.Email(),
                   new IdentityResources.Profile(),
                };
        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("Mango","Mango Server"),
            new ApiScope("read","Read data"),
            new ApiScope("write","Write data"),
            new ApiScope("delete","Delete data"),
        };
        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            { 
                ClientId = "client",
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"read","write","profile"}
            },
             new Client
            {
                ClientId = "mango",
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {"http://localhost:19429/signin-oidc" },
                PostLogoutRedirectUris = {"http://localhost:19429/signout-callback-oidc" },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId, 
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile,
                    "mango"
                }
            }
        };
    }
}
