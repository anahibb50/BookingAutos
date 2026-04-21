using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class ExtraConfiguration : IEntityTypeConfiguration<ExtraEntity>
    {
        public void Configure(EntityTypeBuilder<ExtraEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("EXTRAS", "catalogos");

            // Llave primaria
            builder.HasKey(e => e.id_extra);

            // Propiedades Principales y Unicidad
            builder.Property(e => e.id_extra)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.extra_guid)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");
            builder.HasIndex(e => e.extra_guid).IsUnique().HasDatabaseName("UQ_EXTRAS_GUID");

            builder.Property(e => e.codigo_extra)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            builder.HasIndex(e => e.codigo_extra).IsUnique().HasDatabaseName("UQ_EXTRAS_CODIGO");

            builder.Property(e => e.nombre_extra)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.descripcion_extra)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            builder.Property(e => e.valor_fijo)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            // Estado / Ciclo de Vida
            builder.Property(e => e.estado_extra)
                .IsRequired()
                .HasMaxLength(3)
                .IsFixedLength()
                .IsUnicode(false)
                .HasDefaultValue("ACT");

            builder.Property(e => e.es_eliminado)
                .IsRequired()
                .HasDefaultValue(false);

            // Auditoría
            builder.Property(e => e.fecha_registro_utc)
                .IsRequired()
                .HasColumnType("datetime2(0)")
                .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Property(e => e.creado_por_usuario)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.modificado_por_usuario)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.fecha_modificacion_utc)
                .HasColumnType("datetime2(0)");

            builder.Property(e => e.modificado_desde_ip)
                .HasMaxLength(45)
                .IsUnicode(false);

            builder.Property(e => e.fecha_inhabilitacion_utc)
                .HasColumnType("datetime2(0)");

            builder.Property(e => e.motivo_inhabilitacion)
                .HasMaxLength(200)
                .IsUnicode(false);

            // Integración
            builder.Property(e => e.origen_registro)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            // Concurrencia
            builder.Property(e => e.row_version)
                .IsRowVersion();

            // Relaciones
            builder.HasMany(e => e.ReservasExtras)
                .WithOne(re => re.Extra)
                .HasForeignKey(re => re.id_extra)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}