namespace Booking.Autos.DataAccess.Entities
{
    public class ReservaExtraEntity
    {
        // Identificadores
        public int id_reserva_extra { get; set; }
        public Guid r_x_e_guid { get; set; }

        // Llaves Foráneas (Relación N:M)
        public int id_reserva { get; set; }
        public int id_extra { get; set; }

        // Datos de la transacción
        public int r_x_e_cantidad { get; set; }
        public decimal r_x_e_valor_unitario { get; set; }
        public decimal r_x_e_subtotal { get; set; }

        // Estado y Auditoría
        public string r_x_e_estado { get; set; } = "PEN";
        public DateTime fecha_creacion { get; set; }
        public DateTime fecha_actualizacion { get; set; }
        public DateTime? fecha_eliminacion { get; set; }
        public bool es_eliminado { get; set; }

        // --- Propiedades de Navegación ---

        public virtual ReservaEntity? Reserva { get; set; }
        public virtual ExtraEntity? Extra { get; set; }
    }
}
