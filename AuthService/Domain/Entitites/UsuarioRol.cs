namespace AuthService.Domain.Entities
{
    public class UsuarioRol
    {
        public int UsuarioRolId { get; set; }
        public int UsuarioId { get; set; }
        public int RolId { get; set; }

        public UsuarioSeguridad Usuario { get; set; }
        public Rol Rol { get; set; }

    }
}
