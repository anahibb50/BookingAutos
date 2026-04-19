using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class UsuarioAppConfiguration : IEntityTypeConfiguration<UsuarioAppEntity>
    {
        public void Configure(EntityTypeBuilder<UsuarioAppEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("USUARIOAPP","seguridad");

            // Llave primaria
            builder.HasKey(e => e.id_usuario);

            // Propiedades e Identificación (GUID, Username, Correo)
            builder.Property(e => e.id_usuario)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.usuario_guid)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");
            builder.HasIndex(e => e.usuario_guid).IsUnique().HasDatabaseName("UQ_USUARIOS_APP_GUID");

            builder.Property(e => e.username)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.HasIndex(e => e.username).IsUnique().HasDatabaseName("UQ_USUARIOS_APP_USERNAME");

            builder.Property(e => e.correo)
                .IsRequired()
                .HasMaxLength(120)
                .IsUnicode(false);
            builder.HasIndex(e => e.correo).IsUnique().HasDatabaseName("UQ_USUARIOS_APP_CORREO");

            // Seguridad (Hashes y Salts)
            builder.Property(e => e.password_hash)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);

            builder.Property(e => e.password_salt)
                .IsRequired()
                .HasMaxLength(250)
                .IsUnicode(false);

            // Estado
            builder.Property(e => e.estado_usuario)
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

            // Relación con Cliente (el FK id_cliente que agregaste al final)
            builder.HasOne(e => e.Cliente)
                .WithMany() // O .WithOne() dependiendo de si un cliente puede tener varios usuarios
                .HasForeignKey(e => e.id_cliente)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con la tabla intermedia de Roles
            builder.HasMany(e => e.UsuariosRoles)
                .WithOne(ur => ur.UsuarioApp)
                .HasForeignKey(ur => ur.id_usuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}