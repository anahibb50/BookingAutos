using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class ConductorConfiguration : IEntityTypeConfiguration<ConductorEntity>
    {
        public void Configure(EntityTypeBuilder<ConductorEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("CONDUCTORES", "personas");

            // Llave primaria
            builder.HasKey(e => e.id_conductor);

            // Propiedades Principales y Unicidad
            builder.Property(e => e.id_conductor)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.conductor_guid)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");
            builder.HasIndex(e => e.conductor_guid).IsUnique().HasDatabaseName("UQ_CONDUCTORES_GUID");

            builder.Property(e => e.codigo_conductor)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.HasIndex(e => e.codigo_conductor).IsUnique().HasDatabaseName("UQ_CONDUCTORES_CODIGO");

            builder.Property(e => e.tipo_identificacion)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.numero_identificacion)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);
            builder.HasIndex(e => e.numero_identificacion).IsUnique().HasDatabaseName("UQ_CONDUCTORES_IDENTIFICACION");

            // Nombres y Apellidos
            builder.Property(e => e.con_nombre1)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(false);

            builder.Property(e => e.con_nombre2)
                .HasMaxLength(80)
                .IsUnicode(false);

            builder.Property(e => e.con_apellido1)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(false);

            builder.Property(e => e.con_apellido2)
                .HasMaxLength(80)
                .IsUnicode(false);

            // Licencia y Edad
            builder.Property(e => e.numero_licencia)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);
            builder.HasIndex(e => e.numero_licencia).IsUnique().HasDatabaseName("UQ_CONDUCTORES_LICENCIA");

            builder.Property(e => e.fecha_vencimiento_licencia)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(e => e.edad_conductor)
                .IsRequired()
                .HasColumnType("tinyint");

            // Contacto
            builder.Property(e => e.con_telefono)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.con_correo)
                .IsRequired()
                .HasMaxLength(120)
                .IsUnicode(false);

            // Estado y Ciclo de Vida
            builder.Property(e => e.estado_conductor)
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
            builder.HasMany(e => e.ConductoresReservas)
                .WithOne(cr => cr.Conductor)
                .HasForeignKey(cr => cr.id_conductor)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}