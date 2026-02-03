namespace AuthService.Domain.Entities
{
    public class PermisoComponente
    {
        public int permisoid { get; set; }
        public int rolid { get; set; }
        public int componenteid { get; set; }
        public bool puedever { get; set; }
        public bool puedeeditar { get; set; }
        public bool puedeeliminar { get; set; }

        public Rol Rol { get; set; }
        public ComponentePantalla Componente { get; set; }

    }
}
