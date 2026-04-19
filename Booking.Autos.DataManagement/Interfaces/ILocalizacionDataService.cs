using Booking.Autos.DataManagement.Models.Localizaciones;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface ILocalizacionDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<LocalizacionDataModel>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<LocalizacionDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<LocalizacionDataModel>> GetByCiudadAsync(
            int idCiudad,
            CancellationToken cancellationToken = default);

        Task<LocalizacionDataModel?> GetByNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<LocalizacionDataModel> CreateAsync(
            LocalizacionDataModel model,
            CancellationToken cancellationToken = default);

        Task<LocalizacionDataModel> UpdateAsync(
            LocalizacionDataModel model,
            CancellationToken cancellationToken = default);

        Task<bool> InhabilitarAsync(
            int id,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByNombreEnCiudadAsync(
            string nombre,
            int idCiudad,
            CancellationToken cancellationToken = default);
    }
}