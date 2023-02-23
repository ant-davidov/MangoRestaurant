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
            new ApiScope("mango","mango server"),
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
                RedirectUris={ "https://localhost:44360/signin-oidc", "https://localhost:7085/signin-oidc" },
                PostLogoutRedirectUris={"https://localhost:7085/" },
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
