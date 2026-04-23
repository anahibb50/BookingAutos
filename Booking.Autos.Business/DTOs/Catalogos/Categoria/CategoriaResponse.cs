namespace Booking.Autos.Business.DTOs.Catalogos.Categoria
{
    public class CategoriaResponse
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string Nombre { get; set; }

        public string? Descripcion { get; set; }
    }
}
