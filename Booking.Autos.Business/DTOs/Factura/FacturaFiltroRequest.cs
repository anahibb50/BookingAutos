namespace Booking.Autos.Business.DTOs.Factura
{
    public class FacturaFiltroRequest
    {
        public int? IdCliente { get; set; }
        public int? IdReserva { get; set; }

        public string? Estado { get; set; }

        public DateTime? FechaCreacionDesde { get; set; }
        public DateTime? FechaCreacionHasta { get; set; }

        public decimal? TotalMin { get; set; }
        public decimal? TotalMax { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}