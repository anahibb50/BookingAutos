namespace Booking.Autos.Business.DTOs.Usuario
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string Username { get; set; }
        public string Correo { get; set; }

        public int? IdCliente { get; set; }

        public string Estado { get; set; }
        public bool Activo { get; set; }
    }
}