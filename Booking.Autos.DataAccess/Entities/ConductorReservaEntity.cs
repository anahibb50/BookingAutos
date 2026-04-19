namespace Booking.Autos.DataAccess.Entities
{
    public class ConductorReservaEntity
    {
        public int id_reserva { get; set; }

        public int id_conductor { get; set; }

        public string rol_conductor { get; set; }

        public bool es_principal { get; set; }

        public DateTime fecha_asignacion_utc { get; set; }

        public DateTime? fecha_desasignacion_utc { get; set; }

        public string estado_asignacion { get; set; }

        public string? observaciones { get; set; }

        public string creado_por_usuario { get; set; }

        public DateTime fecha_registro_utc { get; set; }

        public string? modificado_por_usuario { get; set; }

        public DateTime? fecha_modificacion_utc { get; set; }

        public string? modificacion_ip { get; set; }

        public string? servicio_origen { get; set; }

        public DateTime? fecha_eliminacion { get; set; }

        // Navegación
        public virtual ReservaEntity Reserva { get; set; }

        public virtual ConductorEntity Conductor { get; set; }
    }
}