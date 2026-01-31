namespace AuthService.Domain.Entities
{
    public class SubMenu
    {
        public int SubMenuId { get; set; }
        public string Nombre { get; set; }
        public int MenuId { get; set; }

        public Menu Menu { get; set; }
        public ICollection<ComponentePantalla> Componentes { get; set; }

    }
}
