using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs
{
    public class UpdatePasswordRequest
    {
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
