using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class ReservaExtraConfiguration : IEntityTypeConfiguration<ReservaExtraEntity>
    {
        public void Configure(EntityTypeBuilder<ReservaExtraEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("reserva_x_extra", "reservas");

            // Llave primaria
            builder.HasKey(e => e.id_reserva_extra);

            // Propiedades
            builder.Property(e => e.id_reserva_extra)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.r_x_e_guid)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.r_x_e_cantidad)
                .IsRequired()
                .HasDefaultValue(1);

            // Configuración de montos DECIMAL(10,2)
            builder.Property(e => e.r_x_e_valor_unitario)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(e => e.r_x_e_subtotal)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(e => e.r_x_e_estado)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValue("ACT");

            // Auditoría
            builder.Property(e => e.fecha_creacion)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.fecha_actualizacion)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.es_eliminado)
                .IsRequired()
                .HasDefaultValue(false);

            // Relaciones (FKs)
            builder.HasOne(e => e.Reserva)
                .WithMany(r => r.ReservasExtras)
                .HasForeignKey(e => e.id_reserva)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Extra)
                .WithMany(ex => ex.ReservasExtras)
                .HasForeignKey(e => e.id_extra)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}