using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;

namespace AuthService.Infrastructure.Persistence
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AuthDbContext _context;
        public TokenRepository(AuthDbContext context) { _context = context; }

        public void Save(TokenActivo token) { _context.TokensActivos.Add(token); _context.SaveChanges(); }
        public TokenActivo? FindByRefreshToken(string refreshToken) => _context.TokensActivos.FirstOrDefault(t => t.refreshtoken == refreshToken && t.estado == "Activo");
        public void RevokeToken(string accessToken)
        {
            var token = _context.TokensActivos.FirstOrDefault(t => t.accesstoken == accessToken);
            if (token != null) { token.estado = "Revocado"; _context.SaveChanges(); }
        }

    }
}