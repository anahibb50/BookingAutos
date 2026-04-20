using Booking.Autos.Business.DTOs.Reserva;
using Booking.Autos.DataManagement.Models;
using Microservicio.Clientes.DataManagement.Models;

namespace Booking.Autos.Business.Interfaces
{
    public interface IReservaService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<ReservaResponse> CrearAsync(
            CrearReservaRequest request,
            CancellationToken cancellationToken = default);

        Task<ReservaResponse> ActualizarAsync(
            ActualizarReservaRequest request,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<ReservaResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ReservaResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ReservaResponse>> ObtenerPorClienteAsync(
            int idCliente,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ReservaResponse>> ObtenerPorVehiculoAsync(
            int idVehiculo,
            CancellationToken cancellationToken = default);

        // 🔥 PAGINADO (CLAVE)
        Task<DataPagedResult<ReservaResponse>> BuscarAsync(
            ReservaFiltroRequest request,
            CancellationToken cancellationToken = default);

        // =========================
        // DISPONIBILIDAD 🔥🔥🔥
        // =========================

        Task<bool> VerificarDisponibilidadVehiculoAsync(
            int idVehiculo,
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken cancellationToken = default);

        // =========================
        // ACCIONES DE NEGOCIO 🔥🔥🔥
        // =========================

        Task<bool> ConfirmarAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<bool> CancelarAsync(
            int id,
            string motivo,
            CancellationToken cancellationToken = default);
    }
}