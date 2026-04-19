namespace Booking.Autos.DataAccess.Entities
{
    public class CiudadEntity
    {
        public int id_ciudad { get; set; }

        public Guid ciudad_guid { get; set; }

        public string nombre_ciudad { get; set; }

        public string? codigo_postal { get; set; }

        public int id_pais { get; set; }

        public string estado_ciudad { get; set; }

        public string? origen_registro { get; set; }

        public DateTime fecha_creacion { get; set; }

        public DateTime fecha_actualizacion { get; set; }

        public DateTime? fecha_inhabilitacion { get; set; }

        public string? motivo_inhabilitacion { get; set; }

        public bool es_eliminado { get; set; }

        public DateTime? fecha_eliminacion { get; set; }

        // Navegación
        public virtual PaisEntity Pais { get; set; }

        public virtual ICollection<LocalizacionEntity> Localizaciones { get; set; }

        public virtual ICollection<ClienteEntity> Clientes { get; set; }
    }
}