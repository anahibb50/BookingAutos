namespace Booking.Autos.DataManagement.Models.UsuarioApp
{
    public class UsuarioAppDataModel
    {
        // 🔑 Identificación
        public int Id { get; set; }

        public Guid Guid { get; set; }

        // 👤 Usuario
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Correo { get; set; }

        // 🔗 Relación
        public int? IdCliente { get; set; }

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