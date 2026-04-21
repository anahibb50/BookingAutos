using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<CategoriaEntity>
    {
        public void Configure(EntityTypeBuilder<CategoriaEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("categorias","catalogos");

            // Llave primaria
            builder.HasKey(e => e.id_categoria);

            // Propiedades
            builder.Property(e => e.id_categoria)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(e => e.categoria_guid)
                .IsRequired();

            builder.Property(e => e.nombre_categoria)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.fecha_creacion)
                .IsRequired();

            builder.Property(e => e.fecha_actualizacion)
                .IsRequired();

            builder.Property(e => e.fecha_eliminacion);

            builder.Property(e => e.es_eliminado)
                .IsRequired();

            // Relaciones
            builder.HasMany(e => e.Vehiculos)
                .WithOne(v => v.Categoria)
                .HasForeignKey(v => v.id_categoria)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}