namespace Booking.Autos.API.Models.Booking
{
    public class BookingLocalizacionesRequest
    {
        public int? IdCiudad { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 20;
    }
}
