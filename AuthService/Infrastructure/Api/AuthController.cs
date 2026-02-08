using Microsoft.AspNetCore.Mvc;
using AuthService.Application.DTOs;
using AuthService.Application.UseCases;
using AuthService.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.Api
{
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterUser _registerUser;
        private readonly ILoginUser _loginUser;
        private readonly ITokenRepository _tokenRepo;
        private readonly IUsuarioSeguridadRepository _usuarioRepo;
        private readonly IUpdateUserPassword _updatePassword;

        public AuthController(
            IRegisterUser registerUser,
            ILoginUser loginUser,
            IUpdateUserPassword updatePassword,
            ITokenRepository tokenRepo,
            IUsuarioSeguridadRepository usuarioRepo)
        {
            _registerUser = registerUser;
            _loginUser = loginUser;
            _updatePassword = updatePassword;
            _tokenRepo = tokenRepo;
            _usuarioRepo = usuarioRepo;
        }

        [AllowAnonymous]
        [HttpGet("index")]
        public IActionResult Register()
        {
            return Ok("Success...");
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            _registerUser.Execute(request.Username, request.Password);
            return Ok("Usuario registrado correctamente");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var token = _loginUser.Execute(request.Username, request.Password);
            return Ok(token);
        }

        [HttpPost("update-password")]
        public IActionResult UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            _updatePassword.Execute(request.UsuarioId, request.NewPassword);
            return Ok("Contraseña actualizada correctamente");
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshRequest request)
        {
            var oldToken = _tokenRepo.FindByRefreshToken(request.RefreshToken);
            if (oldToken == null || oldToken.Estado != "Activo")
                return Unauthorized("Refresh token inválido o expirado");

            var usuario = _usuarioRepo.FindById(oldToken.UsuarioId);
            if (usuario == null || usuario.EstatusId == 0)
                return Unauthorized("Usuario no válido");

            _tokenRepo.RevokeToken(oldToken.AccessToken);
            var newToken = _loginUser.RefreshExecute(usuario);
            return Ok(newToken);
        }

    }
}