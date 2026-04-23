using System.Text.Json.Serialization;

namespace Booking.Autos.Business.DTOs.Catalogos.Categoria
{
    public class ActualizarCategoriaRequest
    {
        public int Id { get; set; }

        [JsonPropertyName("nombreCategoria")]
        public string Nombre { get; set; }
    }
}
