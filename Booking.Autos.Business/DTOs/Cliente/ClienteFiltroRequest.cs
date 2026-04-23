using System.Text.Json.Serialization;

namespace Booking.Autos.Business.DTOs.Cliente
{
    public class ClienteFiltroRequest
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Identificacion { get; set; }
        public string? TipoIdentificacion { get; set; }
        public int? IdCiudad { get; set; }
        public string? Estado { get; set; }
        public string? Email { get; set; }

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
