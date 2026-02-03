namespace AuthService.Domain.Entities
{
    public class ComponentePantalla
    {
        public int componenteid { get; set; }
        public string nombre { get; set; }
        public int submenuid { get; set; }

        public SubMenu SubMenu { get; set; }
        public ICollection<PermisoComponente> Permisos { get; set; }

    }
}
