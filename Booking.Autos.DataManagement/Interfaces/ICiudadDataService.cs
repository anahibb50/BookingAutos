using Booking.Autos.DataManagement.Models.Ciudades;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface ICiudadDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<CiudadDataModel>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<CiudadDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<CiudadDataModel>> GetByPaisAsync(
            int idPais,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<CiudadDataModel> CreateAsync(
            CiudadDataModel model,
            CancellationToken cancellationToken = default);

        Task<CiudadDataModel> UpdateAsync(
            CiudadDataModel model,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByNombreAsync(
            string nombre,
            int idPais,
            CancellationToken cancellationToken = default);
    }
}