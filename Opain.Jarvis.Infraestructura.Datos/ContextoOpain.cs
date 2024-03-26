using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Opain.Jarvis.Dominio.Entidades;

namespace Opain.Jarvis.Infraestructura.Datos
{
    public class ContextoOpain : IdentityDbContext
        <Usuario, Rol, string,
        ClaimUsuario, RolesUsuarios, LoginUsuario,
        ClaimRol, TokenUsuario>
    {
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Aerolinea> Aerolineas { get; set; }
        public DbSet<Vuelo> Vuelos { get; set; }
        public DbSet<OperacionesVuelo> OperacionesVuelos { get; set; }
        public DbSet<OperacionesVueloMaster> OperacionesVuelosMaster { get; set; }
        public DbSet<Archivo> Archivos { get; set; }
        public DbSet<Pasajero> Pasajeros { get; set; }
        public DbSet<PasajeroTransito> PasajerosTransito { get; set; }
        public DbSet<UsuariosAerolineas> UsuariosAerolineas { get; set; }
        public DbSet<HorarioAerolinea> HorariosAerolineas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TasaAeroportuaria> TasasAeroportuarias { get; set; }
        public DbSet<HorarioOperacion> HorariosOperaciones { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<RespuestaTicket> RespuestasTickets { get; set; }
        public DbSet<Acceso> Accesos { get; set; }

        public DbSet<PoliticasDeTratamientoDeDatos> PoliticasDeTratamientoDeDatos { get; set; }
        public DbSet<CargueArchivo> CargueArchivos { get; set; }
        public DbSet<Causal> Causales { get; set; }
        public DbSet<NovedadProceso> NovedadesProcesos { get; set; }
        public DbSet<Cargue> Cargues { get; set; }

        public DbSet<ValidacionManual> ValidacionesManuales { get; set; }

        public ContextoOpain(DbContextOptions<ContextoOpain> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.ClaimUsuario)
                    .WithOne(e => e.Usuario)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.LoginUsuario)
                    .WithOne(e => e.Usuario)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.TokenUsuario)
                    .WithOne(e => e.Usuario)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.RolesUsuarios)
                    .WithOne(e => e.Usuario)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                b.ToTable("Usuario");

                b.Property(e => e.Id).HasMaxLength(150);
                b.Property(e => e.EmailConfirmed).HasColumnType("int");
                b.Property(e => e.PhoneNumberConfirmed).HasColumnType("int");
                b.Property(e => e.TwoFactorEnabled).HasColumnType("int");
                b.Property(e => e.LockoutEnabled).HasColumnType("int");
                b.Property(e => e.Activo).HasConversion<int>();
            });

            builder.Entity<Rol>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.RolesUsuarios)
                    .WithOne(e => e.Rol)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.ClaimRol)
                    .WithOne(e => e.Rol)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();

                b.ToTable("Roles");

                b.Property(e => e.Id).HasMaxLength(150);
            });

            builder.Entity<ClaimUsuario>(b =>
            {
                b.Property(e => e.Id).HasMaxLength(150);
                b.ToTable("ClaimsUsuario");
            });

            builder.Entity<LoginUsuario>(b =>
            {
                b.Property(e => e.ProviderKey).HasMaxLength(150);
                b.Property(e => e.LoginProvider).HasMaxLength(150);
                b.Property(e => e.UserId).HasMaxLength(150);
                b.ToTable("LoginsUsuario");
            });

            builder.Entity<TokenUsuario>(b =>
            {
                b.Property(e => e.UserId).HasMaxLength(150);
                b.Property(e => e.LoginProvider).HasMaxLength(150);
                b.Property(e => e.Name).HasMaxLength(150);
                b.ToTable("TokensUsuario");
            });

            builder.Entity<ClaimRol>(b =>
            {
                b.Property(e => e.Id).HasMaxLength(150);
                b.ToTable("ClaimsRoles");
            });

            builder.Entity<RolesUsuarios>(b =>
            {
                b.Property(e => e.UserId).HasMaxLength(150);
                b.Property(e => e.RoleId).HasMaxLength(150);
                b.ToTable("RolesUsuarios");
            });


            builder.Entity<Vuelo>()
            .HasOne<Ciudad>(sc => sc.Origen)
            .WithMany(s => s.Origenes)
            .HasForeignKey(sc => sc.IdOrigen);

            builder.Entity<Vuelo>()
           .HasOne<Ciudad>(sc => sc.Destino)
           .WithMany(s => s.Destinos)
           .HasForeignKey(sc => sc.IdDestino);

            builder.Entity<Vuelo>(b =>
            {
                b.Property(e => e.IdEstado).HasConversion<int>();
            });

            builder.Entity<Aerolinea>(b =>
            {
                b.Property(e => e.IdEstado).HasConversion<int>();
                b.Property(e => e.PDFPasajeros).HasConversion<int>();
            });

            builder.Entity<Ciudad>(b =>
            {
                b.Property(e => e.IdEstado).HasConversion<int>();
            });
        }
    }
}
