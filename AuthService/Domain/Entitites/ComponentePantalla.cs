using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entities
{
    public class ComponentePantalla
    {
        [Column("componenteid")]
        public int ComponenteId { get; set; }
        [Required]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;
        [Column("submenuid")]
        public int SubMenuId { get; set; }
        [Required]
        [Column("submenu")]
        public SubMenu SubMenu { get; set; } = null!;
        public ICollection<PermisoComponente> Permisos { get; set; } = new List<PermisoComponente>();
    }
}
