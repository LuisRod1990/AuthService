using AuthService.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

[Table("usuarios", Schema = "dbo")]
public class UsuarioSeguridad
{
    [Column("usuarioid")]
    public int UsuarioId { get; set; }

    [Column("passwordhash")]
    public required string PasswordHash { get; set; }

    [Column("usuario")]
    public required string Usuario { get; set; }

    [Column("estatusid")]
    public short EstatusId { get; set; }

    [Column("fechacreacion")]
    public DateTime FechaCreacion { get; set; }

    [Column("ultimologin")]
    public DateTime? UltimoLogin { get; set; }

    public required ICollection<UsuarioRol> UsuariosRoles { get; set; }
    public required ICollection<TokenActivo> TokensActivos { get; set; }
}