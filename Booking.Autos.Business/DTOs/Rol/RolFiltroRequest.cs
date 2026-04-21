namespace Booking.Autos.Business.DTOs.Rol
{
    public class RolFiltroRequest
    {
        public string? NombreRol { get; set; }

        public bool? Activo { get; set; }

        public string? Estado { get; set; }
    }
}