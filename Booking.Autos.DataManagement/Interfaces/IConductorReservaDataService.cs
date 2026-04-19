using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IConductorReservaDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<ConductorReservaDataModel>> GetByReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ConductorReservaDataModel>> GetByConductorAsync(
            int idConductor,
            CancellationToken cancellationToken = default);

        Task<ConductorReservaDataModel?> GetAsync(
            int idReserva,
            int idConductor,
            CancellationToken cancellationToken = default);

        Task<ConductorReservaDataModel?> GetPrincipalByReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AssignAsync(
            ConductorReservaDataModel model,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
            ConductorReservaDataModel model,
            CancellationToken cancellationToken = default);

        Task RemoveAsync(
            int idReserva,
            int idConductor,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsAsync(
            int idReserva,
            int idConductor,
            CancellationToken cancellationToken = default);
    }
}