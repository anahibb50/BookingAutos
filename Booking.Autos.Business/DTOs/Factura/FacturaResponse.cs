namespace Booking.Autos.Business.DTOs.Factura
{
    public class FacturaResponse
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public int IdReserva { get; set; }
        public int IdCliente { get; set; }

        public string? Descripcion { get; set; }
        public string? Origen { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }

        public string Estado { get; set; }

        public DateTime? FechaAprobacion { get; set; }
        public DateTime? FechaAnulacion { get; set; }

        public string? MotivoAnulacion { get; set; }
    }
}