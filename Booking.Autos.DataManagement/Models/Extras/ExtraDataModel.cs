namespace Booking.Autos.DataManagement.Models.Extras
{
    public class ExtraDataModel
    {
        // 🔑 Identificación
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        // 💰 Valor
        public decimal ValorFijo { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }

        public bool EsEliminado { get; set; }

        // 🧾 Auditoría
        public DateTime FechaRegistroUtc { get; set; }

        public string CreadoPorUsuario { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificadoDesdeIp { get; set; }

        public DateTime? FechaInhabilitacionUtc { get; set; }

        public string OrigenRegistro { get; set; }

        public string? MotivoInhabilitacion { get; set; }

        // 🔒 Concurrencia
        public byte[] RowVersion { get; set; }
    }
}