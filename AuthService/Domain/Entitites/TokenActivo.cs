using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AuthService.Domain.Entities
{
    [Table("tokensactivos", Schema = "dbo")]
    public class TokenActivo
    {
        public int tokenid { get; set; }
        public int usuarioid { get; set; }
        public string accesstoken { get; set; }
        public string refreshtoken { get; set; }
        public DateTime fechacreacion { get; set; }
        public DateTime fechaexpiracion { get; set; }
        public string estado { get; set; }
        //[JsonIgnore]
        public UsuarioSeguridad Usuario { get; set; }

    }
}
