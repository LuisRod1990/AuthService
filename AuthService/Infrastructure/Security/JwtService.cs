using AuthService.Domain.Entities;
using AuthService.Domain.Ports;
using AuthService.Domain.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Infrastructure.Security
{
    public class JwtService : ITokenService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        public JwtService(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }
        public TokenActivo GenerateTokens(UsuarioSeguridad usuario)
        {
            var key = Environment.GetEnvironmentVariable("JWT_KEY");
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("usuarioId", usuario.UsuarioId.ToString()),
                new Claim(ClaimTypes.Name, usuario.Usuario),
            };

            var accessToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: _dateTimeProvider.NowMexico.AddMinutes(12),
                signingCredentials: creds
            );

            var refreshToken = Guid.NewGuid().ToString();

            return new TokenActivo
            {
                UsuarioId = usuario.UsuarioId,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken,
                FechaCreacion = _dateTimeProvider.NowMexico,
                FechaExpiracion = _dateTimeProvider.NowMexico.AddMinutes(12),
                Estado = "Activo"
            };
        }
    }
}
