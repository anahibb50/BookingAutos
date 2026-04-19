using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class ReservaConfiguration : IEntityTypeConfiguration<ReservaEntity>
    {
        public void Configure(EntityTypeBuilder<ReservaEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("Reservas", "reservas");

            // Llave primaria
            builder.HasKey(e => e.id_reserva);

            // Propiedades
            builder.Property(e => e.id_reserva)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(e => e.guid_reserva)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.codigo_reserva)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            // Fechas y Horas
            builder.Property(e => e.fecha_reserva_utc)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.fecha_inicio)
                .IsRequired();

            builder.Property(e => e.fecha_fin)
                .IsRequired();

            builder.Property(e => e.hora_inicio)
                .HasColumnType("time");

            builder.Property(e => e.hora_fin)
                .HasColumnType("time");

            // Montos DECIMAL(10,2)
            builder.Property(e => e.subtotal_reserva)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(e => e.valor_iva)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(e => e.total_reserva)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            // Estados y textos
            builder.Property(e => e.estado_reserva)
                .IsRequired()
                .HasMaxLength(3)
                .IsFixedLength()
                .IsUnicode(false)
                .HasDefaultValue("PEN"); // Pendiente por defecto

            builder.Property(e => e.descripcion_reserva)
                .HasMaxLength(500)
                .IsUnicode(false);

            // Relaciones principales
            builder.HasOne(e => e.Cliente)
                .WithMany(c => c.Reservas)
                .HasForeignKey(e => e.id_cliente)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Vehiculo)
                .WithMany(v => v.Reservas)
                .HasForeignKey(e => e.id_vehiculo)
                .OnDelete(DeleteBehavior.Restrict);

            // Relaciones de Localización (Doble relación a la misma tabla)
            builder.HasOne(e => e.LocalizacionRecogida)
                .WithMany() // No necesita colección inversa específica si no la definiste
                .HasForeignKey(e => e.id_localizacion_recogida)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.LocalizacionEntrega)
                .WithMany()
                .HasForeignKey(e => e.id_localizacion_entrega)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}