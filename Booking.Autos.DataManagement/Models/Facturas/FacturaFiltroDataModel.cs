namespace Booking.Autos.DataManagement.Models.Facturas
{
    public class FacturaFiltroDataModel
    {
        // 🔗 Relaciones
        public int? IdCliente { get; set; }

        public int? IdReserva { get; set; }

        // 🟢 Estado
        public string? Estado { get; set; }

        // 📅 Rangos de fechas
        public DateTime? FechaCreacionDesde { get; set; }

        public DateTime? FechaCreacionHasta { get; set; }

        public DateTime? FechaAprobacionDesde { get; set; }

        public DateTime? FechaAprobacionHasta { get; set; }

        public DateTime? FechaAnulacionDesde { get; set; }

        public DateTime? FechaAnulacionHasta { get; set; }

        // 💰 (Opcional pero PRO)
        public decimal? TotalMin { get; set; }

        public decimal? TotalMax { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}