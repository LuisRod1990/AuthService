namespace AuthService.Domain.Entities
{
    public class TokenActivo
    {
        public int TokenId { get; set; }
        public int UsuarioId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string Estado { get; set; }

        public UsuarioSeguridad Usuario { get; set; }

    }
}
