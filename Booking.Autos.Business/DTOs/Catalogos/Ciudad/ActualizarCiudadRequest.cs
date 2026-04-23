using System.Text.Json.Serialization;

namespace Booking.Autos.Business.DTOs.Ciudad
{
    public class ActualizarCiudadRequest
    {
        public int Id { get; set; }

        [JsonPropertyName("nombreCiudad")]
        public string Nombre { get; set; }

        [JsonPropertyName("codigoPostal")]
        public string? CodigoPostal { get; set; }

        [JsonPropertyName("idPais")]
        public int IdPais { get; set; }
    }
}
