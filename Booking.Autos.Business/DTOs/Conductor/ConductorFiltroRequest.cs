namespace Booking.Autos.Business.DTOs.Conductor
{
    public class ConductorFiltroRequest
    {
        // 🔍 Búsquedas principales
        public string? NumeroIdentificacion { get; set; }
        public string? NumeroLicencia { get; set; }

        // 👤 Nombre (búsqueda parcial)
        public string? Nombre { get; set; }

        // 🟢 Estado
        public string? Estado { get; set; }

        // 📅 Edad (rango opcional)
        public int? EdadMin { get; set; }
        public int? EdadMax { get; set; }

        // 📄 Paginación 🔥
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}