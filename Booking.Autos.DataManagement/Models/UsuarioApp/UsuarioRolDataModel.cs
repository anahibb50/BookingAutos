namespace Booking.Autos.DataManagement.Models.UsuarioApp
{
    public class UsuarioRolDataModel
    {
        // 🔑 Identificación
        public int Id { get; set; }

        // 🔗 Relaciones
        public int IdUsuario { get; set; }

        public int IdRol { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }

        public bool Activo { get; set; }

        public bool EsEliminado { get; set; }

        // 🧾 Auditoría
        public DateTime FechaRegistroUtc { get; set; }

        public string CreadoPorUsuario { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        // 🔒 Concurrencia
        public byte[] RowVersion { get; set; }
    }
}