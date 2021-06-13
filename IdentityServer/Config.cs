// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Extensions;
using System.Linq;
using IdentityServer.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("kanbanboard.admin"),
                new ApiScope("kanbanboard.user"),
            };

        public static IEnumerable<ApiResource> ApiResources => new[]
        {
            new ApiResource("kanbanboard")
            {
                Scopes = new List<string> { "kanbanboard.admin", "kanbanboard.user"},
                ApiSecrets = new List<Secret> {new Secret("1C92337A-A7A0-11EB-BCBC-0242AC130002".Sha256())},
                UserClaims = new List<string> {"role", "openid", "profile" }
              
            }
        };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "angular",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "http://localhost:4200/dashboard" },
                    FrontChannelLogoutUri = "http://localhost:4200/dashboard",
                    PostLogoutRedirectUris = { "http://localhost:4200/dashboard"  },


                    //FrontChannelLogoutUri = "https://localhost:44300/signout-oidc";

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "kanbanboard.user", "kanbanboard.admin" },

                    AllowedCorsOrigins = { "http://localhost:4200"},

                    RequirePkce = true,
                },
            };

        public class IdentityProfileService : IProfileService
        {
            private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
            private readonly UserManager<ApplicationUser> _userManager;

            public IdentityProfileService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, UserManager<ApplicationUser> userManager)
            {
                _claimsFactory = claimsFactory;
                _userManager = userManager;
            }

            public async Task GetProfileDataAsync(ProfileDataRequestContext context)
            {
                var sub = context.Subject.GetSubjectId();
                var user = await _userManager.FindByIdAsync(sub);
                if (user == null)
                {
                    throw new ArgumentException("");
                }

                var principal = await _claimsFactory.CreateAsync(user);
                var claims = principal.Claims.ToList();

                claims.Add(new System.Security.Claims.Claim("is_profile_id", user.Id));

                context.IssuedClaims = claims;
            }

            public async Task IsActiveAsync(IsActiveContext context)
            {
                var sub = context.Subject.GetSubjectId();
                var user = await _userManager.FindByIdAsync(sub);
                context.IsActive = user != null;
            }
        }
    }
}