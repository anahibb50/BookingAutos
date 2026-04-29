namespace Booking.Autos.API.Models.Booking
{
    public class BookingDisponibilidadRequest
    {
        public DateTime FechaRecogida { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public int? IdLocalizacion { get; set; }
    }
}
