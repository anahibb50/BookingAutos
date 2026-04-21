using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Autos.Business.DTOs.ReservaExtra
{
    public class ActualizarReservaExtraDetalleRequest
    {
        public int? Id { get; set; }

        public int IdExtra { get; set; }
        public int Cantidad { get; set; }

        public bool Eliminar { get; set; } // 🔥 clave
    }
}
