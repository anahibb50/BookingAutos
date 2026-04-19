using Booking.Autos.DataAccess.Configurations;
using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Context
{
    public class BookingAutoDbContext : DbContext
    {
        public BookingAutoDbContext(DbContextOptions<BookingAutoDbContext> options)
            : base(options)
        {
        }

        // DbSets para tus tablas principales
        public DbSet<PaisEntity> Paises { get; set; }
        public DbSet<CiudadEntity> Ciudades { get; set; }
        public DbSet<LocalizacionEntity> Localizaciones { get; set; }
        public DbSet<MarcaEntity> Marcas { get; set; }
        public DbSet<CategoriaEntity> Categorias { get; set; }
        public DbSet<VehiculoEntity> Vehiculos { get; set; }
        public DbSet<ClienteEntity> Clientes { get; set; }
        public DbSet<ReservaEntity> Reservas { get; set; }
        public DbSet<ExtraEntity> Extras { get; set; }
        public DbSet<ReservaExtraEntity> ReservaExtras { get; set; }
        public DbSet<ConductorEntity> Conductores { get; set; }
        public DbSet<ConductorReservaEntity> ConductoresReservas { get; set; }
        public DbSet<RolEntity> Roles { get; set; }
        public DbSet<UsuarioAppEntity> UsuariosApp { get; set; }
        public DbSet<UsuarioRolEntity> UsuariosRoles { get; set; }
        public DbSet<AuditoriaEntity> Auditorias { get; set; }

        public DbSet<FacturaEntity> Facturas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar todas las configuraciones del ensamblado actual
            // Esto buscará automáticamente todas las clases que implementen IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingAutoDbContext).Assembly);
        }
    }
}