namespace Booking.Autos.Business.DTOs.Localizacion
{
    public class ActualizarLocalizacionRequest
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public int IdCiudad { get; set; }

        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }

        public string HorarioAtencion { get; set; }
        public string ZonaHoraria { get; set; }
    }
}