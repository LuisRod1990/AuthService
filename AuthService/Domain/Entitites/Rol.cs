namespace AuthService.Domain.Entities
{
    public class Rol
    {
        public int RolId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public ICollection<UsuarioRol> UsuariosRoles { get; set; }

    }
}
