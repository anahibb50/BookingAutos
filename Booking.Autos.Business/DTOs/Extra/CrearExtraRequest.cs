namespace Booking.Autos.Business.DTOs.Extra
{
    public class CrearExtraRequest
    {
        public string Codigo { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public decimal ValorFijo { get; set; }

        public string Estado { get; set; }
    }
}