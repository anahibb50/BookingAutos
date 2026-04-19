namespace Booking.Autos.DataAccess.Entities
{
    public class ConductorEntity
    {
        public int id_conductor { get; set; }

        public Guid conductor_guid { get; set; }

        public string codigo_conductor { get; set; }

        public string tipo_identificacion { get; set; }

        public string numero_identificacion { get; set; }

        public string con_nombre1 { get; set; }

        public string? con_nombre2 { get; set; }

        public string con_apellido1 { get; set; }

        public string? con_apellido2 { get; set; }

        public string numero_licencia { get; set; }

        public DateTime fecha_vencimiento_licencia { get; set; }

        public byte edad_conductor { get; set; }

        public string con_telefono { get; set; }

        public string con_correo { get; set; }

        public string estado_conductor { get; set; }

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
        public virtual ICollection<ConductorReservaEntity> ConductoresReservas { get; set; }
    }
}