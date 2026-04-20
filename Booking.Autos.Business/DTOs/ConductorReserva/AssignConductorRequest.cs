namespace Booking.Autos.Business.DTOs.ConductorReserva
{
    public class AssignConductorRequest
    {
        public int IdReserva { get; set; }
        public int IdConductor { get; set; }

        public string Rol { get; set; } // TITULAR / SECUNDARIO
        public bool EsPrincipal { get; set; }

        public string? Observaciones { get; set; }
    }
}