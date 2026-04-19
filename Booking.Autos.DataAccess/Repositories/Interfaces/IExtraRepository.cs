using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IExtraRepository
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<ExtraEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<ExtraEntity>> GetActivosAsync(CancellationToken cancellationToken = default);

        Task<ExtraEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<ExtraEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

        Task<IEnumerable<ExtraEntity>> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(ExtraEntity extra, CancellationToken cancellationToken = default);

        Task UpdateAsync(ExtraEntity extra, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        // =========================
        // OPERACIÓN ESPECIAL
        // =========================

        Task UpdatePrecioAsync(int id, decimal nuevoPrecio, CancellationToken cancellationToken = default);
    }
}