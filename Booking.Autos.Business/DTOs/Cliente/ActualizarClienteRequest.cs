namespace Booking.Autos.Business.DTOs.Cliente
{
    public class ActualizarClienteRequest
    {
        public int Id { get; set; }

        // 👤 Datos personales
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public string? RazonSocial { get; set; }

        // 📄 Identificación
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }

        // 📍 Ubicación
        public int IdCiudad { get; set; }

        public string? Direccion { get; set; }

        // 👤 Info adicional
        public string Genero { get; set; }

        // 📞 Contacto
        public string? Telefono { get; set; }
        public string? Email { get; set; }
    }
}