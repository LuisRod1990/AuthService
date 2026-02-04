using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entities
{
    public class UsuarioRol
    {
        [Column("usuariorolid")]
        public int UsuarioRolId { get; set; }

        [Column("usuarioid")]
        public int UsuarioId { get; set; }

        [Column("rolid")]
        public int RolId { get; set; }

        public required UsuarioSeguridad Usuario { get; set; }
        public required Rol Rol { get; set; }
    }
}
