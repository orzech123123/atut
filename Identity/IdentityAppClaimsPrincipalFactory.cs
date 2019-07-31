using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Atut.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Atut.Identity
{
    public class IdentityAppClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole>
    {
        public IdentityAppClaimsPrincipalFactory(
            UserManager<User> userManager
            , RoleManager<IdentityRole> roleManager
            , IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        { }

        public async override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.CompanyName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(UserClaimTypes.CompanyName, user.CompanyName)
                });
            }

            if (!string.IsNullOrWhiteSpace(user.CompanyNameShort))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(UserClaimTypes.CompanyNameShort, user.CompanyNameShort)
                });
            }

            if (!string.IsNullOrWhiteSpace(user.Address))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim(UserClaimTypes.Address, user.Address)
                });
            }

            ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim(UserClaimTypes.IsAdmin, Convert.ToString(user.IsAdmin))
            });

            ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                new Claim(UserClaimTypes.CompanyId, Convert.ToString(user.Id))
            });

            return principal;
        }
    }
}