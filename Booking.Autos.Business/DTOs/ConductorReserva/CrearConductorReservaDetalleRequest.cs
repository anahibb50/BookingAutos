namespace Booking.Autos.Business.DTOs.ConductorReserva
{
    public class CrearConductorReservaDetalleRequest
    {
        public int IdConductor { get; set; }

        // Si IdConductor no existe o viene en 0, permite crear el conductor en línea.
        public Booking.Autos.Business.DTOs.Conductor.CrearConductorRequest? NuevoConductor { get; set; }

        public string Rol { get; set; } // TITULAR / SECUNDARIO
        public bool EsPrincipal { get; set; }

        public string? Observaciones { get; set; }
    }
}