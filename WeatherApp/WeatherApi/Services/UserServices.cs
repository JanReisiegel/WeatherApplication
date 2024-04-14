using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Security.Claims;
using WeatherApi.Data;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class UserServices<T> : IProfileService where T : ApplicationUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<T> _claimsFactory;

        public UserServices(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<T> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");
            if (sub == null)
            {
                throw new Exception("No sub Claim provided");
            }
            var user = await _userManager.FindByIdAsync(sub.Value);
            if (user != null)
            {
                context.IssuedClaims.Add(new Claim("email", user.Email));
                context.IssuedClaims.Add(new Claim("username", user.UserName));
                context.IssuedClaims.Add(new Claim("prefered_username", $"{user.FirstName} {user.LastName}"));
                context.IssuedClaims.Add(new Claim("paid", user.PaidAccount ? "true" : "false"));

                List<Claim> userClaims = _userManager.GetClaimsAsync(user).Result.ToList();
                foreach (var claim in userClaims)
                {
                    context.IssuedClaims.Add(new Claim(claim.Type, claim.Value));
                }
            }
        }


        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(0);
        }
    }
}
