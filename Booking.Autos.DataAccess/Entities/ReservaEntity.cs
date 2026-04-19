namespace Booking.Autos.DataAccess.Entities
{
    public class ReservaEntity
    {
        public int id_reserva { get; set; }

        public Guid guid_reserva { get; set; }

        public string codigo_reserva { get; set; }

        public int id_cliente { get; set; }

        public int id_vehiculo { get; set; }

        public int id_localizacion_recogida { get; set; }

        public int id_localizacion_entrega { get; set; }

        public DateTime fecha_reserva_utc { get; set; }

        public DateTime fecha_inicio { get; set; }

        public DateTime fecha_fin { get; set; }

        public TimeSpan? hora_inicio { get; set; }

        public TimeSpan? hora_fin { get; set; }

        public int cantidad_dias { get; set; }

        public decimal subtotal_reserva { get; set; }

        public decimal valor_iva { get; set; }

        public decimal total_reserva { get; set; }

        public string? descripcion_reserva { get; set; }

        public string? origen_canal_reserva { get; set; }

        public string estado_reserva { get; set; }

        public DateTime? fecha_confirmacion_utc { get; set; }

        public DateTime? fecha_cancelacion_utc { get; set; }

        public string? motivo_cancelacion { get; set; }

        // Auditoría
        public string creado_por_usuario { get; set; }

        public string? modificado_por_usuario { get; set; }

        public DateTime? fecha_modificacion_utc { get; set; }

        public string? modificacion_ip { get; set; }

        public string? servicio_origen { get; set; }

        // Soft delete
        public bool es_eliminado { get; set; }

        public DateTime? fecha_eliminacion { get; set; }

        // Navegación
        public virtual ClienteEntity Cliente { get; set; }

        public virtual VehiculoEntity Vehiculo { get; set; }

        public virtual LocalizacionEntity LocalizacionRecogida { get; set; }

        public virtual LocalizacionEntity LocalizacionEntrega { get; set; }

        public virtual ICollection<FacturaEntity> Facturas { get; set; }

        public virtual ICollection<ConductorReservaEntity> ConductoresReservas { get; set; }

        public virtual ICollection<ReservaExtraEntity> ReservasExtras { get; set; }
    }
}