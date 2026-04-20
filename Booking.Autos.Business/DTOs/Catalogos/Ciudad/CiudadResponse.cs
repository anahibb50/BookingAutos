namespace Booking.Autos.Business.DTOs.Catalogos.Ciudad
{
    public class CiudadResponse
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string Nombre { get; set; }
        public string? CodigoPostal { get; set; }

        public int IdPais { get; set; }

        public string Estado { get; set; }
    }
}