namespace Booking.Autos.Business.DTOs.Catalogos.Pais
{
    public class PaisResponse
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string Nombre { get; set; }
        public string? CodigoIso { get; set; }
    }
}