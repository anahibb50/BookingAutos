using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<ClienteEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<ClienteEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<ClienteEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

        Task<ClienteEntity?> GetByIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default);

        Task<ClienteEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(ClienteEntity cliente, CancellationToken cancellationToken = default);

        Task UpdateAsync(ClienteEntity cliente, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default);

        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}