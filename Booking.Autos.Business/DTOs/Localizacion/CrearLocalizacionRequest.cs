namespace Booking.Autos.Business.DTOs.Localizacion
{
    public class CrearLocalizacionRequest
    {
        public string Nombre { get; set; }

        // 🔗 Relación
        public int IdCiudad { get; set; }

        // 📍 Información
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }

        public string HorarioAtencion { get; set; }
        public string ZonaHoraria { get; set; }
    }
}