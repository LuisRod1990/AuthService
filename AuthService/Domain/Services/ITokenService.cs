using AuthService.Domain.Entities;
using AuthService.Domain.Ports;
using System.Security.Claims;
namespace AuthService.Domain.Services
{
    public interface ITokenService
    {
        TokenActivo GenerateTokens(UsuarioSeguridad usuario);
    }
}
