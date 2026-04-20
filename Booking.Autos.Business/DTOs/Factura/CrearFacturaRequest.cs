namespace Booking.Autos.Business.DTOs.Factura
{
    public class CrearFacturaRequest
    {
        public int IdReserva { get; set; }
        public int IdCliente { get; set; }

        public string? Descripcion { get; set; }
        public string? Origen { get; set; }
    }
}