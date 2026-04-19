using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class PaisConfiguration : IEntityTypeConfiguration<PaisEntity>
    {
        public void Configure(EntityTypeBuilder<PaisEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("Paises", "categorias");

            // Llave primaria
            builder.HasKey(e => e.id_pais);

            // Propiedades
            builder.Property(e => e.id_pais)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(e => e.pais_guid)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");

            builder.Property(e => e.nombre_pais)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.codigo_iso)
                .IsRequired()
                .HasMaxLength(3)
                .IsUnicode(false);

            // Auditoría (Siguiendo tu estándar de tablas maestras)
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
            builder.HasMany(e => e.Ciudades)
                .WithOne(c => c.Pais)
                .HasForeignKey(c => c.id_pais)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}