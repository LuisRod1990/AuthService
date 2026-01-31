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
            modelBuilder.Entity<UsuarioSeguridad>().ToTable("Usuarios").HasKey(u => u.UsuarioId);
            modelBuilder.Entity<Rol>().HasKey(r => r.RolId);
            modelBuilder.Entity<UsuarioRol>().HasKey(ur => ur.UsuarioRolId);
            modelBuilder.Entity<Menu>().HasKey(m => m.MenuId);
            modelBuilder.Entity<SubMenu>().HasKey(sm => sm.SubMenuId);
            modelBuilder.Entity<ComponentePantalla>().HasKey(c => c.ComponenteId);
            modelBuilder.Entity<PermisoComponente>().HasKey(p => p.PermisoId);
            modelBuilder.Entity<TokenActivo>().HasKey(t => t.TokenId);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuariosRoles)
                .HasForeignKey(ur => ur.UsuarioId);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.UsuariosRoles)
                .HasForeignKey(ur => ur.RolId);

            modelBuilder.Entity<SubMenu>()
                .HasOne(sm => sm.Menu)
                .WithMany(m => m.SubMenus)
                .HasForeignKey(sm => sm.MenuId);

            modelBuilder.Entity<ComponentePantalla>()
                .HasOne(c => c.SubMenu)
                .WithMany(sm => sm.Componentes)
                .HasForeignKey(c => c.SubMenuId);

            modelBuilder.Entity<PermisoComponente>()
                .HasOne(p => p.Rol)
                .WithMany()
                .HasForeignKey(p => p.RolId);

            modelBuilder.Entity<PermisoComponente>()
                .HasOne(p => p.Componente)
                .WithMany(c => c.Permisos)
                .HasForeignKey(p => p.ComponenteId);

            modelBuilder.Entity<TokenActivo>()
                .HasOne(t => t.Usuario)
                .WithMany()
                .HasForeignKey(t => t.UsuarioId);
        }
    }
}