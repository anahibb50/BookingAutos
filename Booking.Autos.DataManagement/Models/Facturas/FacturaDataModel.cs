namespace Booking.Autos.DataManagement.Models.Facturas
{
    public class FacturaDataModel
    {
        // 🔑 Identificación
        public int Id { get; set; }

        public Guid Guid { get; set; }

        // 🔗 Relaciones
        public int IdReserva { get; set; }

        public int IdCliente { get; set; }

        // 📝 Información
        public string? Descripcion { get; set; }

        public string? Origen { get; set; }

        // 💰 Valores
        public decimal Subtotal { get; set; }

        public decimal Iva { get; set; }

        public decimal Total { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }

        // 📅 Fechas de negocio
        public DateTime? FechaAprobacion { get; set; }

        public DateTime? FechaAnulacion { get; set; }

        public string? MotivoAnulacion { get; set; }

        // 🧾 Auditoría
        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }

        public bool EsEliminado { get; set; }
    }
}