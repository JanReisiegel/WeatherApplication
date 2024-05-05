using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Weather.Models;
using Weather.MyExceptions;
using Weather.ViewModels;

namespace Weather.Services
{
    public static class JsonFileService
    {
        private static readonly string _path = "users.json";
        private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        
        private static async Task<List<ApplicationUser>> ReadFromFileAsync()
        {
            await _lock.WaitAsync();
            try
            {
                if (!File.Exists(_path))
                {
                    return new List<ApplicationUser>();
                }
                var json = await File.ReadAllTextAsync(_path);
                return JsonConvert.DeserializeObject<List<ApplicationUser>>(json);
            }
            finally
            {
                _lock.Release();
            }
        }

        private static async Task WriteToFileAsync(List<ApplicationUser> data)
        {
            await _lock.WaitAsync();
            try
            {
                var json = JsonConvert.SerializeObject(data);
                await File.WriteAllTextAsync(_path, json);
            }
            finally
            {
                _lock.Release();
            }
        }

        public static async Task<ApplicationUser> GetUserAsync(string email)
        {
            var users = await ReadFromFileAsync();
            return users.FirstOrDefault(u => u.Email == email);
        }

        public static async Task<IdentityResult> AddUserAsync(RegisterModel input)
        {
            var users = await ReadFromFileAsync();
            ApplicationUser user = users.FirstOrDefault(u => u.Email == input.Email);
            if (user != null)
            {
                throw new UserException("User already exists");
            }
            var hasher = new PasswordHasher<ApplicationUser>();
            users.Add(new ApplicationUser
            {
                Email = input.Email,
                UserName = input.UserName,
                PhoneNumber = input.PhoneNumber,
                PasswordHash = hasher.HashPassword(null, input.Password),
                NormalizedEmail = input.Email.ToUpper(),
                NormalizedUserName = input.UserName.ToUpper(),
                EmailConfirmed = true,
                PaidAccount = true,
                SavedLocations = new List<Location>()
            });
            await WriteToFileAsync(users);
            return IdentityResult.Success;
        }

        public static async Task<IdentityResult> UpdateUserAsync(UserVM input)
        {
            var users = await ReadFromFileAsync();
            var existingUser = users.FirstOrDefault(u => u.Email == input.Email) ?? throw new UserException("User not found");
            users.Remove(existingUser);
            var hasher = new PasswordHasher<ApplicationUser>();
            var updatedUser = new ApplicationUser
            {
                Email = existingUser.Email,
                UserName = input.UserName,
                PhoneNumber = input.PhoneNumber,
                PasswordHash = hasher.VerifyHashedPassword(null, existingUser.PasswordHash, input.Password) == PasswordVerificationResult.Success ? 
                    existingUser.PasswordHash : 
                    hasher.HashPassword(null, input.Password),
                NormalizedEmail = input.Email.ToUpper(),
                NormalizedUserName = input.UserName.ToUpper(),
                EmailConfirmed = true,
                PaidAccount = input.PaidAccount,
                SavedLocations = input.Locations
            };
            users.Add(updatedUser);
            await WriteToFileAsync(users);
            return IdentityResult.Success;
        }

        public static async Task<IdentityResult> UpdateUserAsync(ApplicationUser input)
        {
            var users = await ReadFromFileAsync();
            var existingUser = users.FirstOrDefault(u => u.Email == input.Email) ?? throw new UserException("User not found");
            users.Remove(existingUser);
            users.Add(input);
            await WriteToFileAsync(users);
            return IdentityResult.Success;
        }

        public static async Task<IdentityResult> DeleteUserAsync(LoginModel input)
        {
            var users = await ReadFromFileAsync();
            var existingUser = users.FirstOrDefault(u => u.Email == input.Email) ?? throw new UserException("User not found");
            var hasher = new PasswordHasher<ApplicationUser>();
            if(hasher.VerifyHashedPassword(null, existingUser.PasswordHash, input.Password) != PasswordVerificationResult.Success)
            {
                throw new UserException("Invalid password");
            }
            users.Remove(existingUser);
            await WriteToFileAsync(users);
            return IdentityResult.Success;
        }

        public static async Task<List<ApplicationUser>> GetUsersAsync()
        {
            var users = await ReadFromFileAsync();
            return users;
        }
    }
}
