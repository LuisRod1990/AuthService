using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AuthService.Domain.Entities
{
    public class ComponentePantalla
    {
        public int componenteid { get; set; }
        [Required]
        public string nombre { get; set; } = string.Empty;
        public int submenuid { get; set; }
        [Required]
        public SubMenu SubMenu { get; set; } = null!;
        public ICollection<PermisoComponente> Permisos { get; set; } = new List<PermisoComponente>();
    }
}
