namespace AuthService.Domain.Entities
{
    public class ComponentePantalla
    {
        public int ComponenteId { get; set; }
        public string Nombre { get; set; }
        public int SubMenuId { get; set; }

        public SubMenu SubMenu { get; set; }
        public ICollection<PermisoComponente> Permisos { get; set; }

    }
}
