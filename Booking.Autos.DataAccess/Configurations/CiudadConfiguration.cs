using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Autos.DataAccess.Configurations
{
    public class CiudadConfiguration : IEntityTypeConfiguration<CiudadEntity>
    {
        public void Configure(EntityTypeBuilder<CiudadEntity> builder)
        {
            // Nombre de la tabla
            builder.ToTable("ciudades","categorias");

            // Llave Primaria
            builder.HasKey(e => e.id_ciudad);

            // Propiedades y Mapeo de Columnas
            builder.Property(e => e.id_ciudad)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.ciudad_guid)
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

            builder.Property(e => e.nombre_ciudad)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.codigo_postal)
                .HasMaxLength(20);

            // CORRECCIÓN: Se llama id_pais y es un INT (FK)
            builder.Property(e => e.id_pais)
                .IsRequired();

            builder.Property(e => e.estado_ciudad)
                .HasMaxLength(3)
                .HasDefaultValue("ACT")
                .IsRequired();

            builder.Property(e => e.origen_registro)
                .HasMaxLength(50);

            builder.Property(e => e.fecha_creacion)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(e => e.fecha_actualizacion)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(e => e.motivo_inhabilitacion)
                .HasMaxLength(255);

            builder.Property(e => e.es_eliminado)
                .HasDefaultValue(false);

            // Índice Único para el GUID
            builder.HasIndex(e => e.ciudad_guid)
                .IsUnique()
                .HasDatabaseName("UQ_ciudad_guid");

            // --- Relación con la tabla Paises ---
            builder.HasOne(c => c.Pais) // Asumiendo que en CiudadEntity tienes: public PaisEntity Pais { get; set; }
                .WithMany(p => p.Ciudades) // Y en PaisEntity: public ICollection<CiudadEntity> Ciudades { get; set; }
                .HasForeignKey(c => c.id_pais)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ciudades_paises");
        }
    }
}