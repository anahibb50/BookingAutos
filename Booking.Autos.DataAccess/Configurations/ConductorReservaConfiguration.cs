using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class ConductorReservaConfiguration : IEntityTypeConfiguration<ConductorReservaEntity>
    {
        public void Configure(EntityTypeBuilder<ConductorReservaEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("conductores_x_reserva", "reservas");

            // Llave primaria (Muchos a Muchos con datos adicionales)
            builder.HasKey(e => new { e.id_reserva, e.id_conductor });

            // Propiedades de negocio
            builder.Property(e => e.rol_conductor)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.es_principal)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.estado_asignacion)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("PEN");

            builder.Property(e => e.observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);

            // Auditoría
            builder.Property(e => e.fecha_registro_utc)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.creado_por_usuario)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.modificacion_ip)
                .HasMaxLength(45)
                .IsUnicode(false);

            builder.Property(e => e.servicio_origen)
                .HasMaxLength(100)
                .IsUnicode(false);

            // Relaciones
            builder.HasOne(e => e.Reserva)
                .WithMany(r => r.ConductoresReservas)
                .HasForeignKey(e => e.id_reserva)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Conductor)
                .WithMany(c => c.ConductoresReservas)
                .HasForeignKey(e => e.id_conductor)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
