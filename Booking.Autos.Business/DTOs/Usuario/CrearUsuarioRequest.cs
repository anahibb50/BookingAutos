namespace Booking.Autos.Business.DTOs.Usuario
{
    public class CrearUsuarioRequest
    {
        public string Username { get; set; }
        public string Correo { get; set; }

        public string Password { get; set; }

        public int? IdCliente { get; set; }
    }
}