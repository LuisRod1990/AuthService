using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entities
{
    [Table("tokensactivos", Schema = "dbo")]
    public class TokenActivo
    {
        [Column("tokenid")]
        public int TokenId { get; set; }

        [Column("usuarioid")]
        public int UsuarioId { get; set; }

        [Column("accesstoken")]
        public required string AccessToken { get; set; }

        [Column("refreshtoken")]
        public required string RefreshToken { get; set; }

        [Column("fechacreacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("fechaexpiracion")]
        public DateTime FechaExpiracion { get; set; }

        [Column("estado")]
        public required string Estado { get; set; }


        public required UsuarioSeguridad Usuario { get; set; }
    }
}

