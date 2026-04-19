namespace Booking.Autos.DataAccess.Entities
{
    public class AuditoriaEntity
    {
        public long id_auditoria { get; set; }

        public Guid auditoria_guid { get; set; }

        public string tabla_afectada { get; set; }

        public string operacion { get; set; }

        public string? id_registro_afectado { get; set; }

        public string? datos_anteriores { get; set; }

        public string? datos_nuevos { get; set; }

        public string usuario_ejecutor { get; set; }

        public string? ip_origen { get; set; }

        public DateTime fecha_evento_utc { get; set; }

        public bool activo { get; set; }

        public byte[] row_version { get; set; }
    }
}