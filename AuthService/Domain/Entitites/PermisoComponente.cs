using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entities
{
    public class PermisoComponente
    {
        [Column("permisoid")]
        public int PermisoId { get; set; }
        [Column("rolid")]
        public int RolId { get; set; }
        [Column("componenteid")]
        public int ComponenteId { get; set; }
        [Column("puedever")]
        public bool PuedeVer { get; set; }
        [Column("puedeeditar")]
        public bool PuedeEditar { get; set; }
        [Column("puedeeliminar")]
        public bool puedeEliminar { get; set; }

        public required Rol Rol { get; set; }
        public required ComponentePantalla Componente { get; set; }
    }
}
