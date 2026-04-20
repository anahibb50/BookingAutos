namespace Booking.Autos.Business.DTOs.Reserva
{
    public class ReservaResponse
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string Codigo { get; set; }

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
        public string Estado { get; set; }

        public DateTime? FechaConfirmacionUtc { get; set; }
        public DateTime? FechaCancelacionUtc { get; set; }

        public string? MotivoCancelacion { get; set; }
    }
}