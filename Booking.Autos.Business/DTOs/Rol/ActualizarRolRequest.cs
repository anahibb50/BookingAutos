namespace Booking.Autos.Business.DTOs.Rol
{
    public class ActualizarRolRequest
    {
        public int IdRol { get; set; }

        public string NombreRol { get; set; } = null!;
        public string? DescripcionRol { get; set; }

        public bool Activo { get; set; }
    }
}