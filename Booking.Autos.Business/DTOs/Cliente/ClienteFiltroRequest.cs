namespace Booking.Autos.Business.DTOs.Cliente
{
    public class ClienteFiltroRequest
    {
        // 🔍 Búsqueda
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }

        public string? Identificacion { get; set; }
        public string? TipoIdentificacion { get; set; }

        // 📍 Ubicación
        public int? IdCiudad { get; set; }

        // 🟢 Estado
        public string? Estado { get; set; }

        // 📞 Contacto
        public string? Email { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}