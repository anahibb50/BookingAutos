using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Autos.Business.DTOs.ConductorReserva
{
    public class ActualizarConductorReservaDetalleRequest
    {
        public int IdReserva { get; set; }
        public int IdConductor { get; set; }

        public string Rol { get; set; } 
        public bool EsPrincipal { get; set; }

        public string? Observaciones { get; set; }

        public bool Eliminar { get; set; }
    }
}
