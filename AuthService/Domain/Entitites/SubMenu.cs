using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entities
{
    public class SubMenu
    {
        [Column("submenuid")]
        public int SubMenuId { get; set; }

        [Column("nombre")]
        public required string Nombre { get; set; }

        [Column("menuid")]
        public int MenuId { get; set; }


        public required Menu Menu { get; set; }
        public required ICollection<ComponentePantalla> Componentes { get; set; }
    }
}
