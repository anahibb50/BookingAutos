namespace Booking.Autos.Business.DTOs.ConductorReserva
{
    public class ConductorReservaDetalleResponse
    {


        // 🔗 Relaciones
        public int IdReserva { get; set; }
        public int IdConductor { get; set; }

        // 🧑‍✈️ Asignación
        public string Rol { get; set; } // TITULAR / SECUNDARIO
        public bool EsPrincipal { get; set; }

        public string? Observaciones { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }
    }
}