using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IConductorRepository
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<ConductorEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<ConductorEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<ConductorEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

        Task<ConductorEntity?> GetByCedulaAsync(string cedula, CancellationToken cancellationToken = default);

        Task<ConductorEntity?> GetByLicenciaAsync(string numLicencia, CancellationToken cancellationToken = default);

        
        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(ConductorEntity conductor, CancellationToken cancellationToken = default);

        Task UpdateAsync(ConductorEntity conductor, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByCedulaAsync(string cedula, CancellationToken cancellationToken = default);

        Task<bool> ExistsByLicenciaAsync(string numLicencia, CancellationToken cancellationToken = default);
    }
}