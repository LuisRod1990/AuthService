using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;

namespace AuthService.Infrastructure.Persistence
{
    public class PermisosRepository : IPermisosRepository
    {
        private readonly AuthDbContext _context;
        public PermisosRepository(AuthDbContext context) { _context = context; }

        public IEnumerable<PermisoComponente> GetPermisosByRol(int rolId) => _context.PermisosComponentes.Where(p => p.rolid == rolId).ToList();
    }
}
