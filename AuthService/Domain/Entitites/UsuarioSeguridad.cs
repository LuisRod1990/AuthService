namespace AuthService.Domain.Entities
{
    public class UsuarioSeguridad
    {
        public int UsuarioId { get; set; }
        public string PasswordHash { get; set; }
        public string Usuario { get; set; }
        public Int16 EstatusId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public ICollection<UsuarioRol> UsuariosRoles { get; set; }
    }
}
