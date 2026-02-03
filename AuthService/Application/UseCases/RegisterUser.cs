using AuthService.Domain.Entities;
using AuthService.Domain.Ports;
using AuthService.Domain.Repositories;
using AuthService.Infrastructure.Security;

namespace AuthService.Application.UseCases
{
    public class RegisterUser : IRegisterUser
    {
        private readonly IUsuarioSeguridadRepository _repo;
        private readonly PasswordHasherService _hasher;
        private readonly IDateTimeProvider _dateTimeProvider;
        public RegisterUser(IUsuarioSeguridadRepository repo, PasswordHasherService hasher, IDateTimeProvider dateTimeProvider)
        {
            _repo = repo;
            _hasher = hasher;
            _dateTimeProvider = dateTimeProvider;
        }
        public void Execute(string username, string password)
        {
            var hash = _hasher.Hash(password);
            var user = new UsuarioSeguridad
            {
                usuario = username
                ,
                passwordhash = hash
                ,
                estatusid = 0
                ,
                fechacreacion = DateTime.UtcNow
            };
            _repo.Save(user);
        }
    }
}