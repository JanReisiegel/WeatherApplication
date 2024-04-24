using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Weather.Models;

namespace Weather.Services
{
    public class UserServices
    {
        private readonly JsonFileService<ApplicationUser> _userStore;
        public UserServices()
        {
            _userStore = new JsonFileService<ApplicationUser>("users.json");
        }
        public string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = "Yxe2g6pX0ftA!#Y1qs@#Z^EyTrT4L1kd";
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("user_id", user.Id),
                    new Claim("email", user.Email),
                    new Claim("paid_account", user.PaidAccount.ToString()),
                    new Claim("username", user.UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public ApplicationUser GetUserByEmail(string email)
        {
            var user = _userStore.ReadFromFileAsync().Result.FirstOrDefault(x => x.Email == email);
            return user;
        }
        public Task UpdateUser(ApplicationUser user)
        {
            var users = _userStore.ReadFromFileAsync().Result;
            users.Remove(users.FirstOrDefault(x => x.Id == user.Id));
            users.Add(user);
            return _userStore.WriteToFileAsync(users);
        }
        public Task DeleteUser(ApplicationUser user)
        {
            var users = _userStore.ReadFromFileAsync().Result;
            users.ForEach(x =>
            {
                if (x.Id == user.Id)
                {
                    users.Remove(x);
                }
            });
            return _userStore.WriteToFileAsync(users);
        }
        public Task<List<ApplicationUser>> GetAllUsers()
        {
            var users = _userStore.ReadFromFileAsync().Result;
            return Task.FromResult(users);
        }
    }
}
