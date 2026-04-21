namespace Booking.Autos.Business.DTOs.UsuarioRol
{
    public class ActualizarUsuarioRolRequest
    {
        public int IdUsuarioRol { get; set; }

        public int IdRol { get; set; }

        public bool Activo { get; set; }
    }
}