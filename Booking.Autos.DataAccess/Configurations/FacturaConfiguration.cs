using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class FacturaConfiguration : IEntityTypeConfiguration<FacturaEntity>
    {
        public void Configure(EntityTypeBuilder<FacturaEntity> builder)
        {
            // 1. Nombre de la tabla
            builder.ToTable("facturas", "reservas");

            // 2. Llave Primaria
            builder.HasKey(e => e.id_factura);

            // 3. Propiedades
            builder.Property(e => e.id_factura)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.factura_guid)
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

            builder.Property(e => e.fac_descripcion)
                .HasMaxLength(255);

            builder.Property(e => e.origen_factura)
                .HasMaxLength(50);

            // --- CONFIGURACIÓN DE DECIMALES (10,2) ---
            builder.Property(e => e.fac_subtotal)
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(e => e.fac_iva)
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(e => e.fac_total)
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0)
                .IsRequired();
            // -----------------------------------------

            builder.Property(e => e.fac_estado)
                .HasMaxLength(3)
                .HasDefaultValue("ABI")
                .IsRequired();

            builder.Property(e => e.motivo_anulacion)
                .HasMaxLength(255);

            // Auditoría
            builder.Property(e => e.fecha_creacion)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(e => e.fecha_actualizacion)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(e => e.es_eliminado)
                .HasDefaultValue(false);

            // 4. Índices Únicos
            builder.HasIndex(e => e.factura_guid)
                .IsUnique()
                .HasDatabaseName("UQ_factura_guid");

            // 5. Relaciones (Foreign Keys)
            builder.HasOne(d => d.Reserva)
                .WithMany(p => p.Facturas)
                .HasForeignKey(d => d.id_reserva)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_factura_reserva");

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.Facturas)
                .HasForeignKey(d => d.id_cliente)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_factura_cliente");
        }
    }
}