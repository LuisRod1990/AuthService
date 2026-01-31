namespace AuthService.Application.DTOs
{
    public class UpdatePasswordRequest
    {
        public int UsuarioId { get; set; }
        public string NewPassword { get; set; }
    }
}
