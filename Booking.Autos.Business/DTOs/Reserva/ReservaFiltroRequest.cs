namespace Booking.Autos.Business.DTOs.Reserva
{
    public class ReservaFiltroRequest
    {
        public int? IdCliente { get; set; }
        public int? IdVehiculo { get; set; }

        public int? IdLocalizacionRecogida { get; set; }
        public int? IdLocalizacionEntrega { get; set; }

        public DateTime? FechaInicioDesde { get; set; }
        public DateTime? FechaInicioHasta { get; set; }

        public DateTime? FechaFinDesde { get; set; }
        public DateTime? FechaFinHasta { get; set; }

        public string? Estado { get; set; }

        public string? CodigoReserva { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}