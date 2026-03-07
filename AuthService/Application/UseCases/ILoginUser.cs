using AuthService.Domain.Entities;

namespace AuthService.Application.UseCases
{
    public interface ILoginUser
    {
        TokenActivo Execute(string username, string password, string city,string country, string explorer, string latitud, string longitud, string publicip, string region);
        TokenActivo RefreshExecute(UsuarioSeguridad usuario, string city, string country, string explorer, string latitud, string longitud, string publicip, string region);
    }
}
