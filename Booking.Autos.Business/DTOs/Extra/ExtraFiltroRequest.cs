namespace Booking.Autos.Business.DTOs.Extra
{
    public class ExtraFiltroRequest
    {
        public string? Nombre { get; set; }
        public string? Estado { get; set; }

        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}