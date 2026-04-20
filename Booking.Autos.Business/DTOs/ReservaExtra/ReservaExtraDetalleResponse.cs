using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Autos.Business.DTOs.ReservaExtra
{
    public class ReservaExtraDetalleResponse
    {
        public int Id { get; set; }
        public int IdReserva { get; set; }

        public int IdExtra { get; set; }
        public string NombreExtra { get; set; }

        public int Cantidad { get; set; }

        public decimal ValorUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
