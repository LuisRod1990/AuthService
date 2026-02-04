using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entities
{
    public class Menu
    {

        [Column("menuid")]
        public int MenuId { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("icono")]
        public string Icono { get; set; } = string.Empty;


        public ICollection<SubMenu> SubMenus { get; set; } = new List<SubMenu>();
    }
}
