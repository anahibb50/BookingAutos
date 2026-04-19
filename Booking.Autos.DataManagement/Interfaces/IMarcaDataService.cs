using Booking.Autos.DataManagement.Models.Marcas;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IMarcaDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<MarcaDataModel>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<MarcaDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<MarcaDataModel?> GetByNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<MarcaDataModel> CreateAsync(
            MarcaDataModel model,
            CancellationToken cancellationToken = default);

        Task<MarcaDataModel> UpdateAsync(
            MarcaDataModel model,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);
    }
}