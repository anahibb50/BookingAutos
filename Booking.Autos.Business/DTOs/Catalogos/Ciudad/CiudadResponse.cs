using System.Text.Json.Serialization;

namespace Booking.Autos.Business.DTOs.Catalogos.Ciudad
{
    public class CiudadResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("guid")]
        public Guid Guid { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("codigoPostal")]
        public string? CodigoPostal { get; set; }

        [JsonPropertyName("idPais")]
        public int IdPais { get; set; }

        [JsonPropertyName("estado")]
        public string Estado { get; set; }
    }
}
