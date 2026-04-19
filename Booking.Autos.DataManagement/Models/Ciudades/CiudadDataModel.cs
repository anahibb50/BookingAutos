namespace Booking.Autos.DataManagement.Models.Ciudades
{
    public class CiudadDataModel
    {
        // 🔑 Identificación
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Nombre { get; set; }

        public string? CodigoPostal { get; set; }

        public int IdPais { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }

        public string? OrigenRegistro { get; set; }

        // 🧾 Auditoría
        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public DateTime? FechaInhabilitacion { get; set; }

        public string? MotivoInhabilitacion { get; set; }

        public bool EsEliminado { get; set; }

        public DateTime? FechaEliminacion { get; set; }
    }
}