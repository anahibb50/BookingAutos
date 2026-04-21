namespace Booking.Autos.Business.DTOs.UsuarioRol
{
    public class CrearUsuarioRolRequest
    {
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }

        public bool Activo { get; set; } = true;
    }
}