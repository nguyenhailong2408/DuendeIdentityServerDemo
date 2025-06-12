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
            new ApiScope("api1", "Demo API 1"),
            new ApiScope("api2", "Demo API 2")
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
                PostLogoutRedirectUris = new List<string> { "https://localhost:44342/auth/callbacklogout" },
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
                PostLogoutRedirectUris = new List<string> { "https://localhost:44357/auth/callbacklogout" },
                AllowedScopes = new List<string> { "openid", "profile", "api2", "offline_access" },
                AllowOfflineAccess = true
            },
            new Client
            {
                //This is declare of all webapi though ApiScope and AllowedScopes
                ClientId = "webapi_all",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "api1", "api2", "offline_access" },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 3600
            }
            };
    }
}
