namespace AuthService.Domain.Entities
{
    public class PermisoComponente
    {
        public int PermisoId { get; set; }
        public int RolId { get; set; }
        public int ComponenteId { get; set; }
        public bool PuedeVer { get; set; }
        public bool PuedeEditar { get; set; }
        public bool PuedeEliminar { get; set; }

        public Rol Rol { get; set; }
        public ComponentePantalla Componente { get; set; }

    }
}
