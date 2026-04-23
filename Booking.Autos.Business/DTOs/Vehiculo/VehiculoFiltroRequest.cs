using System.Text.Json.Serialization;

namespace Booking.Autos.Business.DTOs.Vehiculo
{
    public class VehiculoFiltroRequest
    {
        public int? IdMarca { get; set; }
        public int? IdCategoria { get; set; }

        [JsonPropertyName("idLocalizacion")]
        public int? IdLocalizacion { get; set; }

        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Estado { get; set; }
        public string? TipoTransmision { get; set; }
        public string? TipoCombustible { get; set; }
        public int? CapacidadMinPasajeros { get; set; }
        public bool? AireAcondicionado { get; set; }
        public string? Modelo { get; set; }
        public string? Placa { get; set; }

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
