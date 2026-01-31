namespace AuthService.Domain.Entities
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string Nombre { get; set; }
        public string Icono { get; set; }

        public ICollection<SubMenu> SubMenus { get; set; }

    }
}
