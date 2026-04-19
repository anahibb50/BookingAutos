namespace Booking.Autos.DataManagement.Models.Localizaciones
{
    public class LocalizacionFiltroDataModel
    {
        // 🔍 Búsqueda general
        public string? Nombre { get; set; }

        public string? Codigo { get; set; }

        // 📍 Relación
        public int? IdCiudad { get; set; }

        // 🌎 Zona
        public string? ZonaHoraria { get; set; }

        // 🟢 Estado
        public string? Estado { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}