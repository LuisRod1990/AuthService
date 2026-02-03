using AuthService.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

[Table("usuarios", Schema = "dbo")]
public class UsuarioSeguridad
{
    public int usuarioid { get; set; }
    public string passwordhash { get; set; }
    public string usuario { get; set; }
    public short estatusid { get; set; }
    public DateTime fechacreacion { get; set; }
    public DateTime? ultimologin { get; set; }

    public ICollection<UsuarioRol> UsuariosRoles { get; set; }
    public ICollection<TokenActivo> TokensActivos { get; set; } // 
}