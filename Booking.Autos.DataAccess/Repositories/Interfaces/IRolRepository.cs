using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IRolRepository
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<RolEntity>> GetAllAsync(CancellationToken ct = default);

        Task<RolEntity?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<RolEntity?> GetByNombreAsync(string nombre, CancellationToken ct = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(RolEntity rol, CancellationToken ct = default);

        Task UpdateAsync(RolEntity rol, CancellationToken ct = default);

        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}