namespace Booking.Autos.Business.DTOs.Rol
{
    public class RolResponse
    {
        public int IdRol { get; set; }
        public Guid RolGuid { get; set; }

        public string NombreRol { get; set; } = null!;
        public string? DescripcionRol { get; set; }

        public string Estado { get; set; } = null!;
        public bool Activo { get; set; }

        public DateTime FechaRegistroUtc { get; set; }
    }
}