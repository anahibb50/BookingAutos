namespace Booking.Autos.DataManagement.Models.Reservas
{
    public class ConductorReservaDataModel
    {
        // 🔗 Llave compuesta
        public int IdReserva { get; set; }

        public int IdConductor { get; set; }

        // 👤 Rol
        public string Rol { get; set; }

        public bool EsPrincipal { get; set; }

        // 📅 Fechas
        public DateTime FechaAsignacionUtc { get; set; }

        public DateTime? FechaDesasignacionUtc { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }

        public string? Observaciones { get; set; }

        // 🧾 Auditoría
        public string CreadoPorUsuario { get; set; }

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        public string? ServicioOrigen { get; set; }

        public DateTime? FechaEliminacion { get; set; }
    }
}