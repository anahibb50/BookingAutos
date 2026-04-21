using Booking.Autos.Business.DTOs.ConductorReserva;
using Booking.Autos.Business.DTOs.ReservaExtra;

namespace Booking.Autos.Business.DTOs.Reserva
{
    public class ReservaResponse
    {
        // 🔑 Identificación
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string Codigo { get; set; } = null!;

        // 🔗 Relaciones
        public int IdCliente { get; set; }
        public int IdVehiculo { get; set; }

        public int IdLocalizacionRecogida { get; set; }
        public int IdLocalizacionEntrega { get; set; }

        // 📅 Fechas
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }

        public int CantidadDias { get; set; }

        // 💰 Valores
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }

        // 📝 Info
        public string? Descripcion { get; set; }
        public string? OrigenCanal { get; set; }

        // 🟢 Estado
        public string Estado { get; set; } = null!;

        public DateTime? FechaConfirmacionUtc { get; set; }
        public DateTime? FechaCancelacionUtc { get; set; }

        public string? MotivoCancelacion { get; set; }

        // 🔥 DETALLES (LO QUE TE FALTABA)
        public List<ReservaExtraDetalleResponse>? Extras { get; set; }
        public List<ConductorReservaDetalleResponse>? Conductores { get; set; }
    }
}