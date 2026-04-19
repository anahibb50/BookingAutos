using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface ICiudadRepository
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<CiudadEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<CiudadEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<CiudadEntity?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(CiudadEntity ciudad, CancellationToken cancellationToken = default);

        Task UpdateAsync(CiudadEntity ciudad, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}