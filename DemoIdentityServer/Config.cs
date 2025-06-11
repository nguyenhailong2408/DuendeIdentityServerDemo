using Duende.IdentityServer.Models;

namespace DemoIdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            };
          
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
            new ApiScope("api1", "Demo API")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
            new Client
            {
                ClientId = "webapp1",
                ClientName = "MVC Client 1",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = true,
                ClientSecrets = { new Secret("secret".Sha256()) },
                RedirectUris = new List<string> { "https://localhost:44342/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://localhost:44342/auth/logoutcallback" },
                AllowedScopes = new List<string> { "openid", "profile", "api1", "offline_access" },
                AllowOfflineAccess = true
            },
            new Client
            {
                ClientId = "webapp2",
                ClientName = "MVC Client 2",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = true,
                ClientSecrets = { new Secret("secret".Sha256()) },
                RedirectUris = new List<string> { "https://localhost:44357/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://localhost:44357/auth/logoutcallback" },
                AllowedScopes = new List<string> { "openid", "profile", "api1", "offline_access" },
                AllowOfflineAccess = true
            }
            };
    }
}
