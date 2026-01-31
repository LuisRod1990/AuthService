using AuthService.Domain.Entities;

namespace AuthService.Domain.Repositories
{
    public interface IPermisosRepository
    {
        IEnumerable<PermisoComponente> GetPermisosByRol(int rolId);
    }
}
