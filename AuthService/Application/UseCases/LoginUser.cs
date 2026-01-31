using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using AuthService.Domain.Services;
using AuthService.Infrastructure.Security;
namespace AuthService.Application.UseCases
{
    public class LoginUser : ILoginUser
    {
        private readonly IUsuarioSeguridadRepository _repo;
        private readonly PasswordHasherService _hasher;
        private readonly ITokenService _tokenService;
        private readonly ITokenRepository _tokenRepo;
        public LoginUser(IUsuarioSeguridadRepository repo, PasswordHasherService hasher, ITokenService tokenService, ITokenRepository tokenRepo)
        { _repo = repo; _hasher = hasher; _tokenService = tokenService; _tokenRepo = tokenRepo; }
        public TokenActivo Execute(string username, string password)
        {
            var user = _repo.FindByUsername(username);
            if (user == null || !_hasher.Verify(password, user.PasswordHash))
                throw new Exception("Credenciales inválidas");
            var token = _tokenService.GenerateTokens(user);
            _tokenRepo.Save(token);
            return token;
        }
        public TokenActivo RefreshExecute(UsuarioSeguridad user)
        {
            var token = _tokenService.GenerateTokens(user);
            _tokenRepo.Save(token);
            return token;
        }

    }
}