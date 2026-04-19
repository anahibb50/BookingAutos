using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IReservaExtraDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<ReservaExtraDataModel>> GetByReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<ReservaExtraDataModel>> GetByExtraAsync(
            int idExtra,
            CancellationToken cancellationToken = default);

        Task<ReservaExtraDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<ReservaExtraDataModel> AddAsync(
            ReservaExtraDataModel model,
            CancellationToken cancellationToken = default);

        Task<ReservaExtraDataModel> UpdateAsync(
            ReservaExtraDataModel model,
            CancellationToken cancellationToken = default);

        Task<bool> RemoveAsync(
            int id,
            CancellationToken cancellationToken = default);

        // =========================
        // CÁLCULOS 🔥
        // =========================

        Task<decimal> GetSubtotalByReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);
    }
}