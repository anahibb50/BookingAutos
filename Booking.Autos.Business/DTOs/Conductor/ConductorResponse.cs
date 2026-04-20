namespace Booking.Autos.Business.DTOs.Conductor
{
    public class ConductorResponse
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string Codigo { get; set; }

        // 👤 Nombre completo (🔥 PRO)
        public string Nombre1 { get; set; }
        
        public string Nombre2 { get; set; }

        = string.Empty;

        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
         = string.Empty;

        // 📄 Identificación
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }

        // 🚗 Licencia
        public string NumeroLicencia { get; set; }
        public DateTime FechaVencimientoLicencia { get; set; }

        // 📞 Contacto
        public string Telefono { get; set; }

        public byte Edad { get; set; }
        public string Correo { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }
    }
}