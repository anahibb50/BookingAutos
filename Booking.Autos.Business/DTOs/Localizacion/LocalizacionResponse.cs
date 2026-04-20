namespace Booking.Autos.Business.DTOs.Localizacion
{
    public class LocalizacionResponse
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public string Codigo { get; set; }
        public string Nombre { get; set; }

        // 🔗 Relación
        public int IdCiudad { get; set; }

        // 📍 Información
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }

        public string HorarioAtencion { get; set; }
        public string ZonaHoraria { get; set; }

        // 🟢 Estado
        public string Estado { get; set; }
    }
}