using AuthService.Domain.Entities;

namespace AuthService.Domain.Repositories
{
    public interface IUsuarioSeguridadRepository
    {
        UsuarioSeguridad? FindByUsername(string username);
        UsuarioSeguridad? FindById(int id);
        void Save(UsuarioSeguridad usuario);
        void UpdatePassword(int usuarioId, string newPasswordHash);
    }
}
