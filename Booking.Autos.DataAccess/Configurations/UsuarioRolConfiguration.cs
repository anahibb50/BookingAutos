using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRolEntity>
    {
        public void Configure(EntityTypeBuilder<UsuarioRolEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("USUARIOS_ROLES", "seguridad");

            // Llave primaria
            builder.HasKey(e => e.id_usuario_rol);

            // Propiedades
            builder.Property(e => e.id_usuario_rol)
                .ValueGeneratedOnAdd();

            // Unique Compuesto (id_usuario, id_rol)
            builder.HasIndex(e => new { e.id_usuario, e.id_rol })
                .IsUnique()
                .HasDatabaseName("UQ_USUARIOS_ROLES_USUARIO_ROL");

            // Estado
            builder.Property(e => e.estado_usuario_rol)
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

            // Relaciones
            builder.HasOne(e => e.UsuarioApp)
                .WithMany(u => u.UsuariosRoles)
                .HasForeignKey(e => e.id_usuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Rol)
                .WithMany(r => r.UsuariosRoles)
                .HasForeignKey(e => e.id_rol)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}