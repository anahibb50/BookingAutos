namespace Booking.Autos.DataManagement.Models.Categorias
{
    public class CategoriaDataModel
    {
        // 🔑 Identificación
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        // 🧾 Auditoría REAL de tu tabla
        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool EsEliminado { get; set; }
    }
}