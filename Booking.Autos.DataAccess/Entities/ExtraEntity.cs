namespace Booking.Autos.DataAccess.Entities
{
    public class ExtraEntity
    {
        public int id_extra { get; set; }

        public Guid extra_guid { get; set; }

        public string codigo_extra { get; set; }

        public string nombre_extra { get; set; }

        public string descripcion_extra { get; set; }

        public decimal valor_fijo { get; set; }

        public string estado_extra { get; set; }

        public bool es_eliminado { get; set; }

        public DateTime fecha_registro_utc { get; set; }

        public string creado_por_usuario { get; set; }

        public string? modificado_por_usuario { get; set; }

        public DateTime? fecha_modificacion_utc { get; set; }

        public string? modificado_desde_ip { get; set; }

        public DateTime? fecha_inhabilitacion_utc { get; set; }

        public byte[] row_version { get; set; }

        public string origen_registro { get; set; }

        public string? motivo_inhabilitacion { get; set; }

        // Navegación
        public virtual ICollection<ReservaExtraEntity> ReservasExtras { get; set; }
    }
}