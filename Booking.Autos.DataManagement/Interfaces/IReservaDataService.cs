using Booking.Autos.DataManagement.Models;
using Booking.Autos.DataManagement.Models.Reservas;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IReservaDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IReadOnlyList<ReservaDataModel>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<DataPagedResult<ReservaDataModel>> BuscarAsync(
            ReservaFiltroDataModel filtro,
            CancellationToken cancellationToken = default);

        Task<ReservaDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ReservaDataModel>> GetByClienteAsync(
            int idCliente,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ReservaDataModel>> GetByVehiculoAsync(
            int idVehiculo,
            CancellationToken cancellationToken = default);

        // =========================
        // DISPONIBILIDAD 🔥
        // =========================

        Task<bool> IsVehiculoDisponibleAsync(
            int idVehiculo,
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<ReservaDataModel> CreateAsync(
            ReservaDataModel model,
            CancellationToken cancellationToken = default);

        Task<ReservaDataModel> UpdateAsync(
            ReservaDataModel model,
            CancellationToken cancellationToken = default);

        // =========================
        // ACCIONES DE NEGOCIO
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