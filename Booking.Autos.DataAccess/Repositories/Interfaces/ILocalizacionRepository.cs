using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface ILocalizacionRepository
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<LocalizacionEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<LocalizacionEntity>> GetActivasAsync(CancellationToken cancellationToken = default);

        Task<LocalizacionEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<LocalizacionEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

        Task<IEnumerable<LocalizacionEntity>> GetByCiudadIdAsync(int idCiudad, CancellationToken cancellationToken = default);

        Task<LocalizacionEntity?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(LocalizacionEntity localizacion, CancellationToken cancellationToken = default);

        Task UpdateAsync(LocalizacionEntity localizacion, CancellationToken cancellationToken = default);

        Task InhabilitarAsync(int id, CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByNombreEnCiudadAsync(string nombre, int idCiudad, CancellationToken cancellationToken = default);
    }
}