namespace Booking.Autos.DataManagement.Models.Vehiculos
{
    public class VehiculoFiltroDataModel
    {
        // 🔗 Relaciones
        public int? IdMarca { get; set; }

        public int? IdCategoria { get; set; }

        public int? IdLocalizacion { get; set; }

        // 💰 Precio
        public decimal? PrecioMin { get; set; }

        public decimal? PrecioMax { get; set; }

        // 📅 DISPONIBILIDAD (🔥 CRÍTICO)
        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        // 🟢 Estado
        public string? Estado { get; set; }

        // ⚙️ Características
        public string? TipoTransmision { get; set; }

        public string? TipoCombustible { get; set; }

        public int? CapacidadMinPasajeros { get; set; }

        public bool? AireAcondicionado { get; set; }

        // 🔍 Búsqueda libre
        public string? Modelo { get; set; }

        public string? Placa { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}