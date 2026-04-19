namespace Booking.Autos.DataManagement.Models.Marcas
{
    public class MarcaDataModel
    {
        // 🔑 Identificación
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Nombre { get; set; }

        // 🧾 Auditoría
        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool EsEliminado { get; set; }
    }
}