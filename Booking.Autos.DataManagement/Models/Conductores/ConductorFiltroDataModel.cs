namespace Booking.Autos.DataManagement.Models.Conductores
{
    public class ConductorFiltroDataModel
    {
        // 🔍 Búsqueda por identidad
        public string? TipoIdentificacion { get; set; }

        public string? NumeroIdentificacion { get; set; }

        // 👤 Nombre
        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        // 🚗 Licencia
        public string? NumeroLicencia { get; set; }

        // 🟢 Estado
        public string? Estado { get; set; }

        // 📅 Rango de vencimiento de licencia
        public DateTime? FechaVencimientoDesde { get; set; }

        public DateTime? FechaVencimientoHasta { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}