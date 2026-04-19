using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class AuditoriaConfiguration : IEntityTypeConfiguration<AuditoriaEntity>
    {
        public void Configure(EntityTypeBuilder<AuditoriaEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("AUDITORIA","auditoria");

            // Llave primaria (BIGINT en SQL)
            builder.HasKey(e => e.id_auditoria);

            // Propiedades
            builder.Property(e => e.id_auditoria)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.auditoria_guid)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.tabla_afectada)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.operacion)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.id_registro_afectado)
                .HasMaxLength(100)
                .IsUnicode(false);

            // Mapeo de VARCHAR(MAX)
            builder.Property(e => e.datos_anteriores)
                .IsUnicode(false); // Por defecto sin MaxLength mapea a MAX en SQL

            builder.Property(e => e.datos_nuevos)
                .IsUnicode(false);

            builder.Property(e => e.usuario_ejecutor)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.ip_origen)
                .HasMaxLength(45)
                .IsUnicode(false);

            builder.Property(e => e.fecha_evento_utc)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Concurrencia
            builder.Property(e => e.row_version)
                .IsRowVersion();
        }
    }
}