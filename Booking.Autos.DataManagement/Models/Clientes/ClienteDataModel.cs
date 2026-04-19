namespace Booking.Autos.DataManagement.Models.Clientes
{
    public class ClienteDataModel
    {
        // 🔑 Identificación
        public int Id { get; set; }

        public Guid Guid { get; set; }

        // 👤 Datos personales
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string? RazonSocial { get; set; }

        public string TipoIdentificacion { get; set; }

        public string Identificacion { get; set; }

        public int IdCiudad { get; set; }

        public string? Direccion { get; set; }

        public string Genero { get; set; }

        public string? Telefono { get; set; }

        public string? Email { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }

        // 🧾 Auditoría
        public string CreadoPorUsuario { get; set; }

        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }

        public DateTime? FechaModificacionUtc { get; set; }

        public string? ModificacionIp { get; set; }

        public string? ServicioOrigen { get; set; }

        public bool EsEliminado { get; set; }

        public DateTime? FechaEliminacion { get; set; }
    }
}