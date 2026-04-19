namespace Booking.Autos.DataAccess.Entities
{
    public class ClienteEntity
    {
        public int id_cliente { get; set; }

        public Guid cliente_guid { get; set; }

        public string cli_nombre { get; set; }

        public string cli_apellido { get; set; }

        public string? razon_social { get; set; }

        public string tipo_identificacion { get; set; }

        public string cli_ruc_ced { get; set; }

        public int id_ciudad { get; set; }

        public string? cli_direccion { get; set; }

        public string? cli_genero { get; set; }

        public string? cli_telefono { get; set; }

        public string? cli_email { get; set; }

        public string cli_estado { get; set; }

        // --- 🧾 Campos de Auditoría (Mantenlos aquí si no usas la herencia de AuditoriaEntity) ---
        public string creado_por_usuario { get; set; }
        public DateTime fecha_registro_utc { get; set; }
        public string? modificado_por_usuario { get; set; }
        public DateTime? fecha_modificacion_utc { get; set; }
        public string? modificacion_ip { get; set; }
        public string? servicio_origen { get; set; }

        // --- 🧾 Soft Delete ---
        public bool es_eliminado { get; set; }
        public DateTime? fecha_eliminacion { get; set; }

        // Propiedades de navegación
        public virtual CiudadEntity? Ciudad { get; set; }
        public virtual ICollection<ReservaEntity> Reservas { get; set; } = new List<ReservaEntity>();
        public virtual ICollection<FacturaEntity> Facturas { get; set; } = new List<FacturaEntity>();
    }
}