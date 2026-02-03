using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;

namespace AuthService.Infrastructure.Persistence
{
    public class UsuarioSeguridadRepository : IUsuarioSeguridadRepository
    {
        private readonly AuthDbContext _context;
        public UsuarioSeguridadRepository(AuthDbContext context) { _context = context; }

        public UsuarioSeguridad? FindByUsername(string username) => _context.UsuariosSeguridad.FirstOrDefault(u => u.usuario == username);
        public UsuarioSeguridad? FindById(int id) => _context.UsuariosSeguridad.Find(id);
        public void Save(UsuarioSeguridad usuario) { _context.UsuariosSeguridad.Add(usuario); _context.SaveChanges(); }
        public void UpdatePassword(int usuarioId, string newPasswordHash)
        {
            var user = _context.UsuariosSeguridad.Find(usuarioId);
            if (user != null) { user.passwordhash = newPasswordHash; _context.SaveChanges(); }
        }

    }
}
