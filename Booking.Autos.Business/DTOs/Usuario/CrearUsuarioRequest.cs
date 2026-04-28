namespace Booking.Autos.Business.DTOs.Usuario
{
    public class CrearUsuarioRequest
    {
        public string Username { get; set; }
        public string Correo { get; set; }

        public string Password { get; set; }

        // Datos del cliente para auto-vincular en registro.
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? TipoIdentificacion { get; set; } // CEDULA / RUC / PASAPORTE
        public string? Identificacion { get; set; } // cédula/ruc/pasaporte
        public int? IdCiudad { get; set; }
        public string? Direccion { get; set; }
        public string? Genero { get; set; }
        public string? Telefono { get; set; }
    }
}