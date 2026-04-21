
namespace Booking.Autos.Business.DTOs.ReservaExtra
{
    public class ReservaExtraDetalleResponse
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }
        public int IdReserva { get; set; }

        public int IdExtra { get; set; }
        public string NombreExtra { get; set; } = null!;

        public int Cantidad { get; set; }

        public decimal ValorUnitario { get; set; }
        public decimal Subtotal { get; set; }

        public string Estado { get; set; } = null!;
    }
}