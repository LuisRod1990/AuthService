using AuthService.Domain.Entities;

namespace AuthService.Application.UseCases
{
    public interface ILoginUser
    {
        TokenActivo Execute(string username, string password);
        TokenActivo RefreshExecute(UsuarioSeguridad usuario);
    }
}
