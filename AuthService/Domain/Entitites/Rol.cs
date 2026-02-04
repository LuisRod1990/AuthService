using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entities
{
    public class Rol
    {
        [Column("rolid")]
        public int RolId { get; set; }

        [Column("nombre")]
        public required string Nombre { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }



        public ICollection<UsuarioRol>? UsuariosRoles { get; set; }
    }
}
