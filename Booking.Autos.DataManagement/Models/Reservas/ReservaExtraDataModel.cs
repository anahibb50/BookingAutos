namespace Booking.Autos.DataManagement.Models.Reservas
{
    public class ReservaExtraDataModel
    {
        // 🔑 Identificación
        public int Id { get; set; }

        public Guid Guid { get; set; }

        // 🔗 Relaciones
        public int IdReserva { get; set; }

        public int IdExtra { get; set; }

        // 📦 Datos
        public int Cantidad { get; set; }

        public decimal ValorUnitario { get; set; }

        public decimal Subtotal { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }

        public bool EsEliminado { get; set; }

        // 🧾 Auditoría
        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public DateTime? FechaEliminacion { get; set; }
    }
}