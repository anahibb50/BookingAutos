namespace Booking.Autos.DataAccess.Entities
{
    public class MarcaEntity
    {
        public int id_marca { get; set; }

        public Guid marca_guid { get; set; }

        public string nombre_marca { get; set; }

        public DateTime fecha_creacion { get; set; }

        public DateTime fecha_actualizacion { get; set; }

        public DateTime? fecha_eliminacion { get; set; }

        public bool es_eliminado { get; set; }

        // Navegación
        public virtual ICollection<VehiculoEntity> Vehiculos { get; set; }
    }
}