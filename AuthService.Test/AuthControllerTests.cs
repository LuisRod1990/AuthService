using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using AuthService.Api;
using AuthService.Application.DTOs;
using AuthService.Application.UseCases;
using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;

public class AuthControllerTests
{
    [Fact]
    public void Index_ReturnsSuccess()
    {
        var controller = new AuthController(
            new Mock<IRegisterUser>().Object,
            new Mock<ILoginUser>().Object,
            new Mock<IUpdateUserPassword>().Object,
            new Mock<ITokenRepository>().Object,
            new Mock<IUsuarioSeguridadRepository>().Object
        );

        var result = controller.Register() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal("Success...", result.Value);
    }

    [Fact]
    public void Register_ReturnsOk_WhenUserIsRegistered()
    {
        var mockRegisterUser = new Mock<IRegisterUser>();
        var controller = new AuthController(
            mockRegisterUser.Object,
            new Mock<ILoginUser>().Object,
            new Mock<IUpdateUserPassword>().Object,
            new Mock<ITokenRepository>().Object,
            new Mock<IUsuarioSeguridadRepository>().Object
        );

        var request = new RegisterRequest { Username = "testuser", Password = "password" };
        var result = controller.Register(request) as OkObjectResult;

        mockRegisterUser.Verify(r => r.Execute("testuser", "password"), Times.Once);
        Assert.Equal("Usuario registrado correctamente", result!.Value );
    }

    [Fact]
    public void Login_ReturnsToken_WhenCredentialsAreValid()
    {
        var fakeToken = new TokenActivo
        {
            TokenId = 0,
            UsuarioId = 1,
            AccessToken = "fake-jwt-token",
            RefreshToken = "refresh-token",
            FechaCreacion = DateTime.UtcNow,
            FechaExpiracion = DateTime.UtcNow.AddHours(1),
            Estado = "Activo",
            Usuario = new UsuarioSeguridad
            {
                UsuarioId = 1,
                PasswordHash = "dummyhash",
                Usuario = "testuser",
                EstatusId = 1,
                FechaCreacion = DateTime.UtcNow,
                UsuariosRoles = new List<UsuarioRol>(),
                TokensActivos = new List<TokenActivo>()
            }
        };
        var mockLoginUser = new Mock<ILoginUser>();
        mockLoginUser.Setup(l => l.Execute("testuser", "password")).Returns(fakeToken);

        var controller = new AuthController(
            new Mock<IRegisterUser>().Object,
            mockLoginUser.Object,
            new Mock<IUpdateUserPassword>().Object,
            new Mock<ITokenRepository>().Object,
            new Mock<IUsuarioSeguridadRepository>().Object
        );

        var request = new LoginRequest { Username = "testuser", Password = "password" };
        var result = controller.Login(request) as OkObjectResult;

        Assert.NotNull(result);
        var returnedToken = Assert.IsType<TokenActivo>(result.Value);
        Assert.Equal("fake-jwt-token", returnedToken.AccessToken);
    }

    [Fact]
    public void Login_ThrowsException_WhenCredentialsAreInvalid()
    {
        var mockLoginUser = new Mock<ILoginUser>();
        mockLoginUser.Setup(l => l.Execute("baduser", "badpass"))
                     .Throws(new Exception("Credenciales inválidas"));

        var controller = new AuthController(
            new Mock<IRegisterUser>().Object,
            mockLoginUser.Object,
            new Mock<IUpdateUserPassword>().Object,
            new Mock<ITokenRepository>().Object,
            new Mock<IUsuarioSeguridadRepository>().Object
        );

        var request = new LoginRequest { Username = "baduser", Password = "badpass" };

        Assert.Throws<Exception>(() => controller.Login(request));
    }

    [Fact]
    public void UpdatePassword_ReturnsOk()
    {
        var mockUpdatePassword = new Mock<IUpdateUserPassword>();
        var controller = new AuthController(
            new Mock<IRegisterUser>().Object,
            new Mock<ILoginUser>().Object,
            mockUpdatePassword.Object,
            new Mock<ITokenRepository>().Object,
            new Mock<IUsuarioSeguridadRepository>().Object
        );

        var request = new UpdatePasswordRequest { UsuarioId = 1, NewPassword = "newpass" };
        var result = controller.UpdatePassword(request) as OkObjectResult;

        mockUpdatePassword.Verify(u => u.Execute(1, "newpass"), Times.Once);
        Assert.NotNull(result);
        Assert.Equal("Contraseña actualizada correctamente", result!.Value);
    }

    [Fact]
    public void Refresh_ReturnsUnauthorized_WhenTokenIsInvalid()
    {
        var mockTokenRepo = new Mock<ITokenRepository>();
        mockTokenRepo.Setup(r => r.FindByRefreshToken("badtoken")).Returns((TokenActivo?)null);

        var controller = new AuthController(
            new Mock<IRegisterUser>().Object,
            new Mock<ILoginUser>().Object,
            new Mock<IUpdateUserPassword>().Object,
            mockTokenRepo.Object,
            new Mock<IUsuarioSeguridadRepository>().Object
        );

        var result = controller.Refresh("badtoken") as UnauthorizedObjectResult;

        Assert.NotNull(result);
        Assert.Equal("Refresh token inválido o expirado", result.Value);
    }

    [Fact]
    public void Refresh_ReturnsUnauthorized_WhenUserIsInvalid()
    {
        var fakeToken = new TokenActivo
        {
            TokenId = 0,
            UsuarioId = 1,
            AccessToken = "oldtoken",
            RefreshToken = "refresh-token",
            FechaCreacion = DateTime.UtcNow,
            FechaExpiracion = DateTime.UtcNow.AddHours(1),
            Estado = "Activo",
            Usuario = new UsuarioSeguridad
            {
                UsuarioId = 1,
                PasswordHash = "dummyhash",
                Usuario = "testuser",
                EstatusId = 1,
                FechaCreacion = DateTime.UtcNow,
                UsuariosRoles = new List<UsuarioRol>(),
                TokensActivos = new List<TokenActivo>()
            }
        };
        var mockTokenRepo = new Mock<ITokenRepository>();
        mockTokenRepo.Setup(r => r.FindByRefreshToken("validtoken")).Returns(fakeToken);

        var mockUsuarioRepo = new Mock<IUsuarioSeguridadRepository>();
        mockUsuarioRepo.Setup(r => r.FindById(1)).Returns((UsuarioSeguridad?)null);

        var controller = new AuthController(
            new Mock<IRegisterUser>().Object,
            new Mock<ILoginUser>().Object,
            new Mock<IUpdateUserPassword>().Object,
            mockTokenRepo.Object,
            mockUsuarioRepo.Object
        );

        var result = controller.Refresh("validtoken") as UnauthorizedObjectResult;

        Assert.NotNull(result);
        Assert.Equal("Usuario no válido", result.Value);
    }

    [Fact]
    public void Refresh_ReturnsNewToken_WhenValid()
    {
        var fakeOldToken = new TokenActivo
        {
            TokenId = 0,
            UsuarioId = 1,
            AccessToken = "oldtoken",
            RefreshToken = "refresh-token",
            FechaCreacion = DateTime.UtcNow,
            FechaExpiracion = DateTime.UtcNow.AddHours(1),
            Estado = "Activo",
            Usuario = new UsuarioSeguridad
            {
                UsuarioId = 1,
                PasswordHash = "dummyhash",
                Usuario = "testuser",
                EstatusId = 1,
                FechaCreacion = DateTime.UtcNow,
                UsuariosRoles = new List<UsuarioRol>(),
                TokensActivos = new List<TokenActivo>()
            }
        };
        var fakeUser = new UsuarioSeguridad
        {
            UsuarioId = 1,
            PasswordHash = "dummyhash",
            Usuario = "testuser",
            EstatusId = 1,
            FechaCreacion = DateTime.UtcNow,
            UsuariosRoles = new List<UsuarioRol>(),
            TokensActivos = new List<TokenActivo>()
        };
        var fakeNewToken = new TokenActivo
        {
            TokenId = 0,
            UsuarioId = 1,
            AccessToken = "newtoken",
            RefreshToken = "refresh-token",
            FechaCreacion = DateTime.UtcNow,
            FechaExpiracion = DateTime.UtcNow.AddHours(1),
            Estado = "Activo",
            Usuario = new UsuarioSeguridad
            {
                UsuarioId = 1,
                PasswordHash = "dummyhash",
                Usuario = "testuser",
                EstatusId = 1,
                FechaCreacion = DateTime.UtcNow,
                UsuariosRoles = new List<UsuarioRol>(),
                TokensActivos = new List<TokenActivo>()
            }
        };

        var mockTokenRepo = new Mock<ITokenRepository>();
        mockTokenRepo.Setup(r => r.FindByRefreshToken("validtoken")).Returns(fakeOldToken);

        var mockUsuarioRepo = new Mock<IUsuarioSeguridadRepository>();
        mockUsuarioRepo.Setup(r => r.FindById(1)).Returns(fakeUser);

        var mockLoginUser = new Mock<ILoginUser>();
        mockLoginUser.Setup(l => l.RefreshExecute(fakeUser)).Returns(fakeNewToken);

        var controller = new AuthController(
            new Mock<IRegisterUser>().Object,
            mockLoginUser.Object,
            new Mock<IUpdateUserPassword>().Object,
            mockTokenRepo.Object,
            mockUsuarioRepo.Object
        );

        var result = controller.Refresh("validtoken") as OkObjectResult;

        Assert.NotNull(result);
        var returnedToken = Assert.IsType<TokenActivo>(result.Value);
        Assert.Equal("newtoken", returnedToken.AccessToken);
    }
}