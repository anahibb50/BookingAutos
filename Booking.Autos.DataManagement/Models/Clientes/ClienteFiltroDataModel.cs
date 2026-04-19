namespace Booking.Autos.DataManagement.Models.Clientes
{
    public class ClienteFiltroDataModel
    {
        // 🔍 Búsqueda general
        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public string? Identificacion { get; set; }

        public string? TipoIdentificacion { get; set; }

        // 📍 Ubicación
        public int? IdCiudad { get; set; }

        // 🟢 Estado
        public string? Estado { get; set; }

        // 📧 Contacto
        public string? Email { get; set; }

        // 📄 Paginación (MUY IMPORTANTE)
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}