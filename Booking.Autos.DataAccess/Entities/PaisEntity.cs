namespace Booking.Autos.DataAccess.Entities
{
    public class PaisEntity
    {
        public int id_pais { get; set; }

        public Guid pais_guid { get; set; }

        public string nombre_pais { get; set; }

        public string? codigo_iso { get; set; }

        public DateTime fecha_creacion { get; set; }

        public DateTime fecha_actualizacion { get; set; }

        public DateTime? fecha_eliminacion { get; set; }

        public bool es_eliminado { get; set; }

        // Navegación
        public virtual ICollection<CiudadEntity> Ciudades { get; set; }
    }
}