using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Infrastructure.Security
{
    public class PasswordHasherService
    {
        private readonly PasswordHasher<UsuarioSeguridad> _hasher = new();
        public string Hash(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password);
        public bool Verify(string password, string hash) =>
            BCrypt.Net.BCrypt.Verify(password, hash);

    }
}
