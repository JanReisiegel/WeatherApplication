using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Weather.Models;
using Weather.MyExceptions;

namespace Weather.Services
{
    public class UserServices
    {
        public const string _key = "Yxe2g6pX0ftA!#Y1qs@#Z^EyTrT4L1kd";
        public static string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("user_id", user.Id),
                    new Claim("email", user.Email),
                    //new Claim("paid_account", user.PaidAccount.ToString()),
                    new Claim("username", user.UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static IDictionary<string, string> GetClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParametres = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                tokenHandler.ValidateToken(token, validationParametres, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = new Dictionary<string, string>();
                foreach (var claim in jwtToken.Claims)
                {
                    claims.Add(claim.Type, claim.Value);
                }
                return claims;
            }
            catch(Exception e)
            {
                throw new TokenException(e.Message);
            }
        }

        public static async Task<bool> GetAuthenticate(string userMail)
        {
            var users = await JsonFileService.GetUsersAsync();
            var user = users.FirstOrDefault(u => u.Email == userMail);
            return user != null;
        }
    }
}
