using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class LocalizacionConfiguration : IEntityTypeConfiguration<LocalizacionEntity>
    {
        public void Configure(EntityTypeBuilder<LocalizacionEntity> builder)
        {
            // Nombre de la tabla exacto según tu SQL
            builder.ToTable("LOCALIZACIONES","operacion");

            // Llave primaria
            builder.HasKey(e => e.id_localizacion);

            // --- CAMPOS PRINCIPALES ---
            builder.Property(e => e.id_localizacion)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.localizacion_guid)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.codigo_localizacion)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.nombre_localizacion)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.id_ciudad)
                .IsRequired();

            builder.Property(e => e.direccion_localizacion)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.telefono_contacto)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.correo_contacto)
                .IsRequired()
                .HasMaxLength(120)
                .IsUnicode(false);

            builder.Property(e => e.horario_atencion)
                .IsRequired()
                .HasMaxLength(120)
                .IsUnicode(false);

            builder.Property(e => e.zona_horaria)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            // --- ESTADO / CICLO DE VIDA ---
            builder.Property(e => e.estado_localizacion)
                .IsRequired()
                .HasMaxLength(3)
                .IsFixedLength() // Es CHAR(3) en tu SQL
                .IsUnicode(false)
                .HasDefaultValue("ACT");

            builder.Property(e => e.es_eliminado)
                .IsRequired()
                .HasDefaultValue(false);

            // --- AUDITORÍA ---
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

            // --- INTEGRACIÓN Y OTROS ---
            builder.Property(e => e.origen_registro)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.motivo_inhabilitacion)
                .HasMaxLength(200)
                .IsUnicode(false);

            // --- CONCURRENCIA ---
            builder.Property(e => e.row_version)
                .IsRowVersion()
                .IsConcurrencyToken();

            // --- RELACIONES ---
            builder.HasOne(e => e.Ciudad)
                .WithMany(c => c.Localizaciones)
                .HasForeignKey(e => e.id_ciudad)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices Únicos según tus CONSTRAINTS
            builder.HasIndex(e => e.localizacion_guid).IsUnique();
            builder.HasIndex(e => e.codigo_localizacion).IsUnique();
        }
    }
}