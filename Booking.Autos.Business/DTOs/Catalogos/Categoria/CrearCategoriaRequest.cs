using System.Text.Json.Serialization;

namespace Booking.Autos.Business.DTOs.Catalogos.Categoria
{
    public class CrearCategoriaRequest
    {
        [JsonPropertyName("nombreCategoria")]
        public string Nombre { get; set; }
    }
}
