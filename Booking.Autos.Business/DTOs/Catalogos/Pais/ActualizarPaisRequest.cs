namespace Booking.Autos.Business.DTOs.Pais
{
    public class ActualizarPaisRequest
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string? CodigoIso { get; set; }
    }
}