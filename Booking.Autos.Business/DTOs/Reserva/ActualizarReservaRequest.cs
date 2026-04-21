using Booking.Autos.Business.DTOs.ConductorReserva;
using Booking.Autos.Business.DTOs.ReservaExtra;

namespace Booking.Autos.Business.DTOs.Reserva
{
    public class ActualizarReservaRequest
    {
        public int Id { get; set; }

        // 📅 Fechas
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }

        public int IdLocalizacionRecogida { get; set; }
        public int IdLocalizacionEntrega { get; set; }

        // 📝 Info
        public string? Descripcion { get; set; }

        public List<ActualizarReservaExtraDetalleRequest>? Extras { get; set; }
        public List<ActualizarConductorReservaDetalleRequest>? Conductores { get; set; }
    }
}