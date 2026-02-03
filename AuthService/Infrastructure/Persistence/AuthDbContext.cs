using Microsoft.EntityFrameworkCore;
using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Persistence
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
        public DbSet<UsuarioSeguridad> UsuariosSeguridad { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<UsuarioRol> UsuariosRoles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<SubMenu> SubMenus { get; set; }
        public DbSet<ComponentePantalla> ComponentesPantalla { get; set; }
        public DbSet<PermisoComponente> PermisosComponentes { get; set; }
        public DbSet<TokenActivo> TokensActivos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioSeguridad>().ToTable("usuarios", "dbo").HasKey(u => u.usuarioid);
            modelBuilder.Entity<Rol>().HasKey(r => r.rolid);
            modelBuilder.Entity<UsuarioRol>().HasKey(ur => ur.usuariorolid);
            modelBuilder.Entity<Menu>().HasKey(m => m.menuid);
            modelBuilder.Entity<SubMenu>().HasKey(sm => sm.submenuid);
            modelBuilder.Entity<ComponentePantalla>().HasKey(c => c.componenteid);
            modelBuilder.Entity<PermisoComponente>().HasKey(p => p.permisoid);
            modelBuilder.Entity<TokenActivo>().HasKey(t => t.tokenid);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.usuario)
                .WithMany(u => u.UsuariosRoles)
                .HasForeignKey(ur => ur.usuarioid);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.rol)
                .WithMany(r => r.UsuariosRoles)
                .HasForeignKey(ur => ur.rolid);

            modelBuilder.Entity<SubMenu>()
                .HasOne(sm => sm.Menu)
                .WithMany(m => m.SubMenus)
                .HasForeignKey(sm => sm.menuid);

            modelBuilder.Entity<ComponentePantalla>()
                .HasOne(c => c.SubMenu)
                .WithMany(sm => sm.Componentes)
                .HasForeignKey(c => c.submenuid);

            modelBuilder.Entity<PermisoComponente>()
                .HasOne(p => p.Rol)
                .WithMany()
                .HasForeignKey(p => p.rolid);

            modelBuilder.Entity<PermisoComponente>()
                .HasOne(p => p.Componente)
                .WithMany(c => c.Permisos)
                .HasForeignKey(p => p.componenteid);

            modelBuilder.Entity<TokenActivo>()
                .HasOne(t => t.Usuario)
                .WithMany(u => u.TokensActivos)
                .HasForeignKey(t => t.usuarioid);

            modelBuilder.Entity<UsuarioSeguridad>()
                .Property(u => u.fechacreacion)
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity<UsuarioSeguridad>()
                .Property(u => u.ultimologin)
                .HasColumnType("timestamp with time zone");

        }
    }
}