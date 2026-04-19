using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IReservaExtraRepository
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<ReservaExtraEntity>> GetByReservaAsync(int idReserva, CancellationToken cancellationToken = default);

        Task<IEnumerable<ReservaExtraEntity>> GetByExtraAsync(int idExtra, CancellationToken cancellationToken = default);

        Task<ReservaExtraEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(ReservaExtraEntity reservaExtra, CancellationToken cancellationToken = default);

        Task UpdateAsync(ReservaExtraEntity reservaExtra, CancellationToken cancellationToken = default);

        Task RemoveAsync(int idReservaExtra, CancellationToken cancellationToken = default);

        // =========================
        // NEGOCIO / CÁLCULOS
        // =========================

        Task<decimal> GetSubtotalByReservaAsync(int idReserva, CancellationToken cancellationToken = default);
    }
}