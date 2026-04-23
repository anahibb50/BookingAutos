using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public int Page { get; set; } = 1;

        [JsonIgnore]
        public int PageSize { get; set; } = 10;

        [JsonPropertyName("pagina")]
        public int? Pagina
        {
            set
            {
                if (value.HasValue)
                    Page = value.Value;
            }
        }

        [JsonPropertyName("tamano")]
        public int? Tamano
        {
            set
            {
                if (value.HasValue)
                    PageSize = value.Value;
            }
        }
    }
}
