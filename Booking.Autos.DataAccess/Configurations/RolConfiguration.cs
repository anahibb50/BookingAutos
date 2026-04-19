using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<RolEntity>
    {
        public void Configure(EntityTypeBuilder<RolEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("ROL","seguridad");

            // Llave primaria
            builder.HasKey(e => e.id_rol);

            // Propiedades e Identificación
            builder.Property(e => e.id_rol)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.rol_guid)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");
            builder.HasIndex(e => e.rol_guid).IsUnique().HasDatabaseName("UQ_ROLES_GUID");

            builder.Property(e => e.nombre_rol)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.HasIndex(e => e.nombre_rol).IsUnique().HasDatabaseName("UQ_ROLES_NOMBRE");

            builder.Property(e => e.descripcion_rol)
                .HasMaxLength(200)
                .IsUnicode(false);

            // Estado
            builder.Property(e => e.estado_rol)
                .IsRequired()
                .HasMaxLength(3)
                .IsFixedLength()
                .IsUnicode(false)
                .HasDefaultValue("ACT");

            builder.Property(e => e.es_eliminado)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.activo)
                .IsRequired()
                .HasDefaultValue(true);

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

            // Concurrencia
            builder.Property(e => e.row_version)
                .IsRowVersion();

            // Relaciones (Asumiendo que un Rol tiene muchos Usuarios)
            builder.HasMany(e => e.UsuariosRoles)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.id_rol)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}