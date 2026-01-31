using AuthService.Domain.Repositories;
using AuthService.Infrastructure.Security;

namespace AuthService.Application.UseCases
{
    public class UpdateUserPassword : IUpdateUserPassword
    {
        private readonly IUsuarioSeguridadRepository _repo;
        private readonly PasswordHasherService _hasher;
        public UpdateUserPassword(IUsuarioSeguridadRepository repo, PasswordHasherService hasher) { _repo = repo; _hasher = hasher; }
        public void Execute(int usuarioId, string newPassword)
        {
            var hash = _hasher.Hash(newPassword);
            _repo.UpdatePassword(usuarioId, hash);
        }
    }
}
