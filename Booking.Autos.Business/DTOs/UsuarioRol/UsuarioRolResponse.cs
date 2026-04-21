namespace Booking.Autos.Business.DTOs.UsuarioRol
{
    public class UsuarioRolResponse
    {
        public int IdUsuarioRol { get; set; }

        public int IdUsuario { get; set; }
        public int IdRol { get; set; }

        public string Estado { get; set; } = null!;
        public bool Activo { get; set; }

        public DateTime FechaRegistroUtc { get; set; }
    }
}