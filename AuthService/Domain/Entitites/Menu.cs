namespace AuthService.Domain.Entities
{
    public class Menu
    {
        public int menuid { get; set; }
        public string nombre { get; set; }
        public string icono { get; set; }

        public ICollection<SubMenu> SubMenus { get; set; }

    }
}
