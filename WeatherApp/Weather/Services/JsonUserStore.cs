using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Security.Claims;
using Weather.Models;

namespace Weather.Services
{
    public class JsonUserStore : IUserStore<ApplicationUser>,
        IUserPasswordStore<ApplicationUser>,
        IUserEmailStore<ApplicationUser>
    {
        private readonly string _usersFilePath;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        public JsonUserStore()
        {
            _usersFilePath = "users.json";
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            await _lock.WaitAsync();
            try
            {
                var users = await ReadFromFileAsync();
                users.Add(user);
                await WriteToFileAsync(users);
                return IdentityResult.Success;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            await _lock.WaitAsync();
            try
            {
                var users = await ReadFromFileAsync();
                var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    users.Remove(existingUser);
                    await WriteToFileAsync(users);
                    return IdentityResult.Success;
                }
                return IdentityResult.Failed(new IdentityError { Description = $"User with id {user.Id} not found" });
            }
            finally
            {
                _lock.Release();
            }
        }

        public void Dispose()
        {
            
        }

        public Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var users = ReadFromFileAsync().Result;
            return Task.FromResult(users.FirstOrDefault(u => u.NormalizedEmail == normalizedEmail));
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var users = await ReadFromFileAsync();
            return users.FirstOrDefault(u => u.Id == userId);
        }

        public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var users = await ReadFromFileAsync();
            return users.FirstOrDefault(u => u.NormalizedUserName == normalizedUserName);
        }

        public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var users = ReadFromFileAsync().Result;
            var resultUser = users.FirstOrDefault(u => u.Id == user.Id);
            return Task.FromResult(resultUser?.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var users = ReadFromFileAsync().Result;
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
            return Task.FromResult(existingUser?.EmailConfirmed ?? false);
        }

        public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var users = ReadFromFileAsync().Result;
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
            return Task.FromResult(existingUser?.NormalizedEmail);
        }

        public async Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var users = await ReadFromFileAsync();
            var resultUser = users.FirstOrDefault(u => u.Id == user.Id);
            return resultUser?.NormalizedUserName;
        }

        public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var users = ReadFromFileAsync().Result;
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
            return Task.FromResult(existingUser?.PasswordHash);
        }

        public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var users = await ReadFromFileAsync();
            var resultUser = users.FirstOrDefault(u => u.Email == user.Email);
            return resultUser?.Id;
        }

        public async Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var users = await ReadFromFileAsync();
            var resultUser = users.FirstOrDefault(u => u.Id == user.Id);
            return resultUser?.UserName;
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var users = ReadFromFileAsync().Result;
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
            return Task.FromResult(existingUser?.PasswordHash != null);
        }

        public async Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
        {
            await _lock.WaitAsync();
            try
            {
                var users = await ReadFromFileAsync();
                var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    existingUser.Email = email;
                    await WriteToFileAsync(users);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            await _lock.WaitAsync();
            try
            {
                var users = await ReadFromFileAsync();
                var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    existingUser.EmailConfirmed = confirmed;
                    await WriteToFileAsync(users);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            await _lock.WaitAsync();
            try
            {
                var users = await ReadFromFileAsync();
                var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    existingUser.NormalizedEmail = normalizedEmail;
                    await WriteToFileAsync(users);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            await _lock.WaitAsync();
            try
            {
                var users = await ReadFromFileAsync();
                var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    existingUser.NormalizedUserName = normalizedName;
                    await WriteToFileAsync(users);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            await _lock.WaitAsync();
            try
            {
                var users = await ReadFromFileAsync();
                var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    existingUser.PasswordHash = passwordHash;
                    await WriteToFileAsync(users);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
        {
            await _lock.WaitAsync();
            try
            {
                var users = await ReadFromFileAsync();
                var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    existingUser.UserName = userName;
                    await WriteToFileAsync(users);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            await _lock.WaitAsync();
            try
            {
                var users = await ReadFromFileAsync();
                var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    users.Remove(existingUser);
                    users.Add(user);
                    await WriteToFileAsync(users);
                    return IdentityResult.Success;
                }
                return IdentityResult.Failed(new IdentityError { Description = $"User with id {user.Id} not found" });
            }
            finally
            {
                _lock.Release();
            }
        }
        public async Task<List<ApplicationUser>> ReadFromFileAsync()
        {
            if (!File.Exists(_usersFilePath))
            {
                return new List<ApplicationUser>();
            }
            var json = await File.ReadAllTextAsync(_usersFilePath);
            return JsonConvert.DeserializeObject<List<ApplicationUser>>(json);
        }

        public async Task WriteToFileAsync(List<ApplicationUser> users)
        {
            var json = JsonConvert.SerializeObject(users);
            await File.WriteAllTextAsync(_usersFilePath, json);
        }
    }
}
