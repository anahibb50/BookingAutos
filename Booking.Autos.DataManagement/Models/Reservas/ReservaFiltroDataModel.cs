namespace Booking.Autos.DataManagement.Models.Reservas
{
    public class ReservaFiltroDataModel
    {
        // 🔗 Relaciones
        public int? IdCliente { get; set; }

        public int? IdVehiculo { get; set; }

        public int? IdLocalizacionRecogida { get; set; }

        public int? IdLocalizacionEntrega { get; set; }

        // 📅 Rango de fechas (🔥 clave)
        public DateTime? FechaInicioDesde { get; set; }

        public DateTime? FechaInicioHasta { get; set; }

        public DateTime? FechaFinDesde { get; set; }

        public DateTime? FechaFinHasta { get; set; }

        // 🟢 Estado
        public string? Estado { get; set; }

        // 📅 Estado de negocio
        public DateTime? FechaConfirmacionDesde { get; set; }

        public DateTime? FechaConfirmacionHasta { get; set; }

        public DateTime? FechaCancelacionDesde { get; set; }

        public DateTime? FechaCancelacionHasta { get; set; }

        // 🔍 Búsqueda
        public string? CodigoReserva { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}