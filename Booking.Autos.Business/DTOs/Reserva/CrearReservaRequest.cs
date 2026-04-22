using Booking.Autos.Business.DTOs.ConductorReserva;
using Booking.Autos.Business.DTOs.ReservaExtra;

namespace Booking.Autos.Business.DTOs.Reserva
{
    public class CrearReservaRequest
    {
        // 🔗 Relaciones
        public int IdCliente { get; set; }
        public int IdVehiculo { get; set; }

        public int IdLocalizacionRecogida { get; set; }
        public int IdLocalizacionEntrega { get; set; }

        public int CantidadDias { get; set; }

        // 📅 Fechas
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }

        // 📝 Info
        public string? Descripcion { get; set; }

        // 🔥 DETALLES
        public List<CrearReservaExtraDetalleRequest>? Extras { get; set; }
        public List<CrearConductorReservaDetalleRequest> Conductores { get; set; }
    }
}