using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;

namespace AuthService.Infrastructure.Persistence
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AuthDbContext _context;
        public TokenRepository(AuthDbContext context) { _context = context; }

        public void Save(TokenActivo token) 
        {
            var fechaLimite = DateTime.UtcNow.Date.AddDays(-2);

            var otrosTokens = _context.TokensActivos
                .Where(t => t.UsuarioId == token.UsuarioId
                            && t.Estado == "Activo"
                            && t.FechaCreacion.Date <= fechaLimite)
                .ToList();


            foreach (var t in otrosTokens)
            {
                t.Estado = "Revocado";
            }

            // Insertar el nuevo token
            _context.TokensActivos.Add(token);

            // Guardar cambios
            _context.SaveChanges();
        }

        // LARJ: public TokenActivo? FindByRefreshToken(string refreshToken) => _context.TokensActivos.FirstOrDefault(t => t.RefreshToken == refreshToken && t.Estado == "Activo"); -- Para varios usuarios
        // LARJ: solo para el portafolio, para permitir una única sesión por usuario, se debe usar la anteiror
        public TokenActivo? FindByRefreshToken(string refreshToken) 
            => _context.TokensActivos.FirstOrDefault(t => t.RefreshToken == refreshToken);
        public void RevokeToken(string accessToken)
        {
            var token = _context.TokensActivos.FirstOrDefault(t => t.AccessToken == accessToken);
            if (token != null) { token.Estado = "Revocado"; _context.SaveChanges(); }
        }

    }
}