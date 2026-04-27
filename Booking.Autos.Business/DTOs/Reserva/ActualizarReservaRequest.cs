namespace Booking.Autos.Business.DTOs.Reserva
{
    public class ActualizarReservaRequest
    {
        public int Id { get; set; }

        // Relaciones editables
        public int IdCliente { get; set; }
        public int IdVehiculo { get; set; }

        public int CantidadDias { get; set; }

        // Fechas
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }

        public int IdLocalizacionRecogida { get; set; }
        public int IdLocalizacionEntrega { get; set; }

        // Info
        public string? Descripcion { get; set; }
    }
}
