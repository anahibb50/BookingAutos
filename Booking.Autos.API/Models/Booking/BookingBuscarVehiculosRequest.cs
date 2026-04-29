namespace Booking.Autos.API.Models.Booking
{
    public class BookingBuscarVehiculosRequest
    {
        public int? IdLocalizacion { get; set; }
        public DateTime? FechaRecogida { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 20;
    }
}
