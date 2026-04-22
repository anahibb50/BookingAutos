namespace Booking.Autos.Business.DTOs.Conductor
{
    public class ActualizarConductorRequest
    {
        public int Id { get; set; }

        // 👤 Nombres
        public string Nombre1 { get; set; }
        public string? Nombre2 { get; set; }

        public string Apellido1 { get; set; }
        public string? Apellido2 { get; set; }

        // 📄 Identificación
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }

        // 🚗 Licencia
        public string NumeroLicencia { get; set; }
        public DateTime FechaVencimientoLicencia { get; set; }

        public byte Edad { get; set; }

        // 📞 Contacto
        public string Telefono { get; set; }
        public string Correo { get; set; }

        // 🟢 Estado
        public string Estado { get; set; } = "ACT";
    }
}