namespace Booking.Autos.Business.DTOs.Ciudad
{
    public class CrearCiudadRequest
    {
        public string Nombre { get; set; }

        // 🔗 Relación
        public int IdPais { get; set; }
    }
}