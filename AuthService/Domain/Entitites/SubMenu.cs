namespace AuthService.Domain.Entities
{
    public class SubMenu
    {
        public int submenuid { get; set; }
        public string nombre { get; set; }
        public int menuid { get; set; }

        public Menu Menu { get; set; }
        public ICollection<ComponentePantalla> Componentes { get; set; }

    }
}
