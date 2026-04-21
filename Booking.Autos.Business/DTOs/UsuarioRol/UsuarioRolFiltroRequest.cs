namespace Booking.Autos.Business.DTOs.UsuarioRol
{
    public class UsuarioRolFiltroRequest
    {
        public int? IdUsuario { get; set; }
        public int? IdRol { get; set; }

        public bool? Activo { get; set; }
        public string? Estado { get; set; }
    }
}