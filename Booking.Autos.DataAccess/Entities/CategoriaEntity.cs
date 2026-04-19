namespace Booking.Autos.DataAccess.Entities
{
    public class CategoriaEntity
    {
        public int id_categoria { get; set; }

        public Guid categoria_guid { get; set; }

        public string nombre_categoria { get; set; }

        public DateTime fecha_creacion { get; set; }

        public DateTime fecha_actualizacion { get; set; }

        public DateTime? fecha_eliminacion { get; set; }

        public bool es_eliminado { get; set; }

        // Navegación
        public virtual ICollection<VehiculoEntity> Vehiculos { get; set; }
    }
}