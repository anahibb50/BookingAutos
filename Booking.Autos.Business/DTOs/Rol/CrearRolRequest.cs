namespace Booking.Autos.Business.DTOs.Rol
{
    public class CrearRolRequest
    {
        public string NombreRol { get; set; } = null!;
        public string? DescripcionRol { get; set; }

        public bool Activo { get; set; } = true;
    }
}