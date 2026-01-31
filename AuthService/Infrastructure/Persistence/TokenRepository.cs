using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;

namespace AuthService.Infrastructure.Persistence
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AuthDbContext _context;
        public TokenRepository(AuthDbContext context) { _context = context; }

        public void Save(TokenActivo token) { _context.TokensActivos.Add(token); _context.SaveChanges(); }
        public TokenActivo? FindByRefreshToken(string refreshToken) => _context.TokensActivos.FirstOrDefault(t => t.RefreshToken == refreshToken && t.Estado == "Activo");
        public void RevokeToken(string accessToken)
        {
            var token = _context.TokensActivos.FirstOrDefault(t => t.AccessToken == accessToken);
            if (token != null) { token.Estado = "Revocado"; _context.SaveChanges(); }
        }

    }
}