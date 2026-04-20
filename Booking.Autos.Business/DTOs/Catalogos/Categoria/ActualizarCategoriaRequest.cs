namespace Booking.Autos.Business.DTOs.Catalogos.Categoria
{
    public class ActualizarCategoriaRequest
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string? Descripcion { get; set; }

        public string Estado { get; set; }
    }
}