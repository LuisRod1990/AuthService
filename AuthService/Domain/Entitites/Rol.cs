namespace AuthService.Domain.Entities
{
    public class Rol
    {
        public int rolid { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }

        public ICollection<UsuarioRol> UsuariosRoles { get; set; }

    }
}
