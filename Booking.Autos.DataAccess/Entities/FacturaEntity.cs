namespace Booking.Autos.DataAccess.Entities
{
    public class FacturaEntity
    {
        public int id_factura { get; set; }

        public Guid factura_guid { get; set; }

        public int id_reserva { get; set; }

        public int id_cliente { get; set; }

        public string? fac_descripcion { get; set; }

        public string? origen_factura { get; set; }

        public decimal fac_subtotal { get; set; }

        public decimal fac_iva { get; set; }

        public decimal fac_total { get; set; }

        public string fac_estado { get; set; }

        public DateTime? fecha_aprobacion { get; set; }

        public DateTime? fecha_anulacion { get; set; }

        public string? motivo_anulacion { get; set; }

        public DateTime fecha_creacion { get; set; }

        public DateTime fecha_actualizacion { get; set; }

        public DateTime? fecha_eliminacion { get; set; }

        public bool es_eliminado { get; set; }

        // Navegación
        public virtual ReservaEntity Reserva { get; set; }

        public virtual ClienteEntity Cliente { get; set; }
    }
}