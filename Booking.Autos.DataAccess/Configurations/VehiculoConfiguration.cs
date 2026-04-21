using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class VehiculoConfiguration : IEntityTypeConfiguration<VehiculoEntity>
    {
        public void Configure(EntityTypeBuilder<VehiculoEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("Vehiculos","catalogos");

            // Llave primaria
            builder.HasKey(e => e.id_vehiculo);

            // Propiedades de identidad y GUID
            builder.Property(e => e.id_vehiculo)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(e => e.vehiculo_guid)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            // Campos de texto con sus longitudes según la imagen
            builder.Property(e => e.codigo_interno_vehiculo)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.placa_vehiculo)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            builder.Property(e => e.modelo_vehiculo)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.color_vehiculo)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.tipo_combustible)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.tipo_transmision)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            // Tipos numéricos específicos (SmallInt y TinyInt)
            builder.Property(e => e.anio_fabricacion)
                .IsRequired()
                .HasColumnType("smallint");

            builder.Property(e => e.capacidad_pasajeros)
                .IsRequired()
                .HasColumnType("tinyint");

            builder.Property(e => e.capacidad_maletas)
                .IsRequired()
                .HasColumnType("tinyint");

            builder.Property(e => e.numero_puertas)
                .IsRequired()
                .HasColumnType("tinyint");

            // Precio y Kilometraje
            builder.Property(e => e.precio_base_dia)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(e => e.kilometraje_actual)
                .IsRequired();

            // Observaciones e Imagen (permiten nulos)
            builder.Property(e => e.observaciones_generales)
                .HasMaxLength(300)
                .IsUnicode(false);

            builder.Property(e => e.imagen_referencial_url)
                .HasMaxLength(300)
                .IsUnicode(false);

            builder.Property(e => e.estado_vehiculo)
                .IsRequired()
                .HasMaxLength(3)
                .IsFixedLength()
                .IsUnicode(false);

            builder.Property(e => e.aire_acondicionado)
                .IsRequired()
                .HasDefaultValue(true);

            // Auditoría y Concurrencia
            builder.Property(e => e.es_eliminado)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.fecha_registro_utc)
                .IsRequired()
                .HasColumnType("datetime2(0)")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.row_version)
                .IsRowVersion();

            // Relaciones
            builder.HasOne(e => e.Marca)
                .WithMany(m => m.Vehiculos)
                .HasForeignKey(e => e.id_marca)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Categoria)
                .WithMany(c => c.Vehiculos)
                .HasForeignKey(e => e.id_categoria)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Localizacion)
                .WithMany(l => l.Vehiculos)
                .HasForeignKey(e => e.localizacion_actual)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}