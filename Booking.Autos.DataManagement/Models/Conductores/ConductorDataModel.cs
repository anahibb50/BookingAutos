namespace Booking.Autos.DataManagement.Models.Conductores
{
    public class ConductorDataModel
    {
        // 🔑 Identificación
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Codigo { get; set; }

        // 📄 Identificación personal
        public string TipoIdentificacion { get; set; }

        public string NumeroIdentificacion { get; set; }

        // 👤 Nombres
        public string Nombre1 { get; set; }

        public string? Nombre2 { get; set; }

        public string Apellido1 { get; set; }

        public string? Apellido2 { get; set; }

        // 🚗 Licencia
        public string NumeroLicencia { get; set; }

        public DateTime FechaVencimientoLicencia { get; set; }

        public byte Edad { get; set; }

        // 📞 Contacto
        public string Telefono { get; set; }

        public string Correo { get; set; }

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