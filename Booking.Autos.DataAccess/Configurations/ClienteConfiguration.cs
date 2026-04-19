using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<ClienteEntity>
    {
        public void Configure(EntityTypeBuilder<ClienteEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("clientes","personas");

            // Llave primaria
            builder.HasKey(e => e.id_cliente);

            // Propiedades y Unicidad
            builder.Property(e => e.id_cliente)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.cliente_guid)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.HasIndex(e => e.cliente_guid)
                .IsUnique()
                .HasDatabaseName("UQ_cliente_guid");

            // Datos Personales
            builder.Property(e => e.cli_nombre)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.cli_apellido)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.razon_social)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder.Property(e => e.tipo_identificacion)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.cli_ruc_ced)
                .IsRequired()
                .HasMaxLength(13)
                .IsUnicode(false);

            builder.HasIndex(e => e.cli_ruc_ced)
                .IsUnique()
                .HasDatabaseName("UQ_cliente_identificacion");

            builder.Property(e => e.cli_direccion)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.cli_genero)
                .HasMaxLength(1)
                .IsFixedLength()
                .IsUnicode(false);

            builder.Property(e => e.cli_telefono)
                .HasMaxLength(15)
                .IsUnicode(false);

            builder.Property(e => e.cli_email)
                .HasMaxLength(150)
                .IsUnicode(false);

            builder.Property(e => e.cli_estado)
                .IsRequired()
                .HasMaxLength(3)
                .IsFixedLength()
                .IsUnicode(false)
                .HasDefaultValue("ACT");

            // Auditoría
            builder.Property(e => e.creado_por_usuario)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.fecha_registro_utc)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.modificado_por_usuario)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.fecha_modificacion_utc)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.modificacion_ip)
                .HasMaxLength(45)
                .IsUnicode(false);

            builder.Property(e => e.servicio_origen)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.es_eliminado)
                .IsRequired()
                .HasDefaultValue(false);

            // Relaciones
            builder.HasOne(e => e.Ciudad)
                .WithMany(c => c.Clientes)
                .HasForeignKey(e => e.id_ciudad)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Reservas)
                .WithOne(r => r.Cliente)
                .HasForeignKey(r => r.id_cliente)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}