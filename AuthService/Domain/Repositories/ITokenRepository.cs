using AuthService.Domain.Entities;

namespace AuthService.Domain.Repositories
{
    public interface ITokenRepository
    {
        void Save(TokenActivo token);
        TokenActivo? FindByRefreshToken(string refreshToken);
        void RevokeToken(string accessToken);

    }
}
