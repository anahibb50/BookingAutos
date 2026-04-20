namespace Booking.Autos.Business.DTOs.Ciudad
{
    public class ActualizarCiudadRequest
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int IdPais { get; set; }
    }
}