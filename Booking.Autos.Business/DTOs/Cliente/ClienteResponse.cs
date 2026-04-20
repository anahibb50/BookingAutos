namespace Booking.Autos.Business.DTOs.Cliente
{
    public class ClienteResponse
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        // 👤 Info
        public string Nombre{ get; set; }
        public string Apellido { get; set; }

        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }

        // 📍 Ubicación
        public int IdCiudad { get; set; }

        // 📞 Contacto
        public string? Telefono { get; set; }
        public string? Email { get; set; }


        // 🟢 Estado
        public string Estado { get; set; }
    }
}