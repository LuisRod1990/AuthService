using AuthService.Application.DTOs;
using AuthService.Application.UseCases;
using AuthService.Domain.Repositories;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Responses;
using UAParser;

namespace AuthService.Api
{
    [Authorize]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AuthController> _logger;
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
            IUsuarioSeguridadRepository usuarioRepo,
            IWebHostEnvironment env,
            ILogger<AuthController> logger
            )
        {
            _logger = logger;
            _registerUser = registerUser;
            _loginUser = loginUser;
            _updatePassword = updatePassword;
            _tokenRepo = tokenRepo;
            _usuarioRepo = usuarioRepo;
            _env = env;
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
            try
            {
                // 1. IP del cliente
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "N/A";
                if (ipAddress == "::1" || ipAddress == "127.0.0.1")
                {
                    // Puedes poner cualquier IP pública para pruebas
                    ipAddress = "189.203.45.12"; // Ejemplo: IP de México
                }
                // 2. User-Agent (navegador)
                string uaString = Request.Headers["User-Agent"].ToString();
                var parser = Parser.GetDefault();
                var clientInfo = parser.Parse(uaString);
                string browser = clientInfo.UA.Family ?? "Unknown";
                string browserVersion = clientInfo.UA.Major ?? "Unknown";
                string os = clientInfo.OS.Family ?? "Unknown";
                // 3. Ruta absoluta al archivo mmdb
                string dbPath = Path.Combine(_env.ContentRootPath, "App_Data", "GeoLite2-City.mmdb");
                using var reader = new DatabaseReader(dbPath);
                var city = reader.City(ipAddress);
                // Convertir todo a string para guardar en BD
                string country = city.Country.Name ?? "Unknown";
                string region = city.MostSpecificSubdivision.Name ?? "Unknown";
                string cityName = city.City.Name ?? "Unknown";
                string latitude = city.Location.Latitude.ToString() ?? "Unknown";
                string longitude = city.Location.Longitude.ToString() ?? "Unknown";
                var token = _loginUser.Execute(request.Username, request.Password, cityName, country, browser, latitude, longitude, ipAddress, region);
                return Ok(token);
            }
            catch (Exception ex)
            {
                // Aquí capturas cualquier error inesperado
                _logger.LogError(ex, "Error en Login()");
                return StatusCode(500, "Ocurrió un error interno");
            }
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
            try
            {
                // 1. IP del cliente
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "N/A";
                // 2. User-Agent (navegador)
                string uaString = Request.Headers["User-Agent"].ToString();
                var parser = Parser.GetDefault();
                var clientInfo = parser.Parse(uaString);
                string browser = clientInfo.UA.Family ?? "Unknown";
                string browserVersion = clientInfo.UA.Major ?? "Unknown";
                string os = clientInfo.OS.Family ?? "Unknown";
                // 3. Ruta absoluta al archivo mmdb
                string dbPath = Path.Combine(_env.ContentRootPath, "App_Data", "GeoLite2-City.mmdb");
                using var reader = new DatabaseReader(dbPath);
                var city = reader.City(ipAddress);
                // Convertir todo a string para guardar en BD
                string country = city.Country.Name ?? "Unknown";
                string region = city.MostSpecificSubdivision.Name ?? "Unknown";
                string cityName = city.City.Name ?? "Unknown";
                string latitude = city.Location.Latitude.ToString() ?? "Unknown";
                string longitude = city.Location.Longitude.ToString() ?? "Unknown";
                var oldToken = _tokenRepo.FindByRefreshToken(request.RefreshToken);
                // LARJ: solo para el portafolio, para permitir una única sesión por usuario, se debe usar la anteiror
                //if (oldToken == null || oldToken.Estado.ToUpper() != "ACTIVO")
                if (oldToken == null)
                {
                    _logger.LogError("Intento de refresh con token inválido o expirado");
                    return Unauthorized("Refresh token inválido o expirado");
                }

                var usuario = _usuarioRepo.FindById(oldToken.UsuarioId);
                if (usuario == null || usuario.EstatusId == 0)
                {
                    _logger.LogError($"Usuario no válido. Id: {oldToken.UsuarioId}");
                    return Unauthorized("Usuario no válido");
                }

                _tokenRepo.RevokeToken(oldToken.AccessToken);
                var newToken = _loginUser.RefreshExecute(usuario, cityName, country, browser, latitude, longitude, ipAddress, region);

                _logger.LogInformation($"Token refrescado correctamente para usuario {usuario.Usuario}");
                return Ok(newToken);
            }
            catch (Exception ex)
            {
                // Aquí capturas cualquier error inesperado
                _logger.LogError(ex, "Error en Refresh()");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

    }
}