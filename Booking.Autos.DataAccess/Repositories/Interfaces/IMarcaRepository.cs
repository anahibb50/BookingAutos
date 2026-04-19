using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IMarcaRepository
    {
        // =========================
        // CONSULTAS BÁSICAS
        // =========================

        Task<IEnumerable<MarcaEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<MarcaEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<MarcaEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

        Task<MarcaEntity?> GetByNombreAsync(string nombreMarca, CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(MarcaEntity marca, CancellationToken cancellationToken = default);

        Task UpdateAsync(MarcaEntity marca, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByNombreAsync(string nombreMarca, CancellationToken cancellationToken = default);

        Task<bool> TieneVehiculosAsociadosAsync(int idMarca, CancellationToken cancellationToken = default);
    }
}