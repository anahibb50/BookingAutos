namespace Booking.Autos.Business.DTOs.ReservaExtra
{
    public class AddExtraRequest
    {
        public int IdReserva { get; set; }
        public int IdExtra { get; set; }

        public int Cantidad { get; set; }
    }
}