using System.Text.Json.Serialization;

namespace Booking.Autos.Business.DTOs.Conductor
{
    public class ConductorFiltroRequest
    {
        public string? NumeroIdentificacion { get; set; }
        public string? NumeroLicencia { get; set; }
        public string? Nombre { get; set; }
        public string? Estado { get; set; }
        public int? EdadMin { get; set; }
        public int? EdadMax { get; set; }

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
