using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IConductorReservaRepository
    {
        // =========================
        // CONSULTAS POR RELACIÓN
        // =========================

        Task<IEnumerable<ConductorReservaEntity>> GetByReservaIdAsync(int idReserva, CancellationToken cancellationToken = default);

        Task<IEnumerable<ConductorReservaEntity>> GetByConductorIdAsync(int idConductor, CancellationToken cancellationToken = default);

        // =========================
        // LLAVE COMPUESTA
        // =========================

        Task<ConductorReservaEntity?> GetByIdsAsync(int idReserva, int idConductor, CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(ConductorReservaEntity conductorReserva, CancellationToken cancellationToken = default);

        Task UpdateAsync(ConductorReservaEntity conductorReserva, CancellationToken cancellationToken = default);

        Task DeleteAsync(int idReserva, int idConductor, CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS DE NEGOCIO
        // =========================

        Task<ConductorReservaEntity?> GetConductorPrincipalByReservaIdAsync(int idReserva, CancellationToken cancellationToken = default);

        Task<bool> IsConductorAssignedToReservaAsync(int idReserva, int idConductor, CancellationToken cancellationToken = default);
    }
}