namespace AuthService.Domain.Entities
{
    public class UsuarioRol
    {
        public int usuariorolid { get; set; }
        public int usuarioid { get; set; }
        public int rolid { get; set; }

        public UsuarioSeguridad usuario { get; set; }
        public Rol rol { get; set; }

    }
}
