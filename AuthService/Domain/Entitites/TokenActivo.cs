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



        [Column("explorer")]
        public required string Explorer { get; set; }

        [Column("publicip")]
        public required string PublicIp { get; set; }

        [Column("country")]
        public required string Country { get; set; }

        [Column("region")]
        public required string Region { get; set; }

        [Column("city")]
        public required string City { get; set; }

        [Column("latitud")]
        public required string Latitud { get; set; }

        [Column("longitud")]
        public required string Longitud { get; set; }





        public required UsuarioSeguridad Usuario { get; set; }
    }
}

