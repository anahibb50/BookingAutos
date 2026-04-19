using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class MarcaConfiguration : IEntityTypeConfiguration<MarcaEntity>
    {
        public void Configure(EntityTypeBuilder<MarcaEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("Marcas", "categorias");

            // Llave primaria
            builder.HasKey(e => e.id_marca);

            // Propiedades
            builder.Property(e => e.id_marca)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(e => e.marca_guid)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.nombre_marca)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            // Auditoría
            builder.Property(e => e.fecha_creacion)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.fecha_actualizacion)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.fecha_eliminacion)
                .IsRequired(false);

            builder.Property(e => e.es_eliminado)
                .IsRequired()
                .HasDefaultValue(false);

            // Relaciones
            builder.HasMany(e => e.Vehiculos)
                .WithOne(v => v.Marca)
                .HasForeignKey(v => v.id_marca)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}