namespace Booking.Autos.DataManagement.Models.Localizaciones
{
    public class LocalizacionDataModel
    {
        // 🔑 Identificación
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        // 🔗 Relación
        public int IdCiudad { get; set; }

        // 📍 Información
        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Correo { get; set; }

        public string HorarioAtencion { get; set; }

        public string ZonaHoraria { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }

        public bool EsEliminado { get; set; }

        // 🧾 Auditoría
        public DateTime FechaRegistroUtc { get; set; }

        public string CreadoPorUsuario { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificadoDesdeIp { get; set; }

        public DateTime? FechaInhabilitacionUtc { get; set; }

        public string OrigenRegistro { get; set; }

        public string? MotivoInhabilitacion { get; set; }

        // 🔒 Concurrencia
        public byte[] RowVersion { get; set; }
    }
}