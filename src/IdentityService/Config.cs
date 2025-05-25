using Duende.IdentityServer.Models;

namespace IdentityService;

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
            new ApiScope("auctionApp", "Auction app full access")
        };

    public static IEnumerable<Client> Clients(IConfiguration config) =>
        new Client[]
        {
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman",

                AllowedGrantTypes = {GrantType.ResourceOwnerPassword},
                ClientSecrets = { new Secret("NotASecret".Sha256()) },

                AllowedScopes = { "openid", "profile", "auctionApp" },
                RedirectUris = {"htttps://wwww.getpostman.com/oath2/callback"}
            },
            new Client
            {
                ClientId = "nextApp",
                ClientName = "nextApp",

                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                RequirePkce = false,
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedScopes = { "openid", "profile", "auctionApp" },
                RedirectUris = {config["ClientApp"] + "/api/auth/callback/id-server"},
                AllowOfflineAccess = true,
                AccessTokenLifetime = 3600*24*30,
                AlwaysIncludeUserClaimsInIdToken = true 
            }

          
        };
}
