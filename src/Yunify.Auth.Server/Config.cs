using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Yunify.Auth.Server.Model;

namespace Yunify.Auth.Server
{
    public class Config
    {
        public static IEnumerable<ApiResourceModel> GetApiResources()
        {
            return new List<ApiResourceModel>
            {
                new ApiResourceModel(
                    new ApiResource {
                        Name = "api1",
                        DisplayName = "My API",
                        Scopes =
                        {
                            new Scope()
                            {
                                Name = "api1",
                                DisplayName = "Full access to API 1",
                                UserClaims = { "serial" }
                            }
                        },
                        UserClaims = { "serial" }
                    }
                )
            };
        }

        public static IEnumerable<ClientModel> GetClients()
        {
            return new List<ClientModel>
            {
                new ClientModel(new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "api1" },
                    AccessTokenLifetime = 300
                })
            };
        }

        public static IEnumerable<IdentityResourceModel> GetIdentityResources()
        {
            return new List<IdentityResourceModel>
            {
                new IdentityResourceModel(new IdentityResources.OpenId()),
                new IdentityResourceModel(new IdentityResources.Profile()),
                new IdentityResourceModel(new IdentityResources.Email()),
            };
        }

        public static List<SerialModel> GetSerials()
        {
            return new List<SerialModel>
            {
                new SerialModel("1", true),
                new SerialModel("2", true)
            };
        }

        public static List<UserModel> GetUsers()
        {
            var users = new List<UserModel>
            {
                new UserModel
                {
                    UserId = "12345",
                    Serial = "1",
                    UserName = "alice"
                },
                new UserModel
                {
                    UserId = "67890",
                    Serial = "2",
                    UserName = "bob"
                }
            };

            var hasher = new PasswordHasher<UserModel>();

            users.ForEach(u => u.PasswordHash = hasher.HashPassword(u, "password"));

            return users;
        }
    }
}