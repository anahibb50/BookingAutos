using Booking.Autos.DataManagement.Models.Paises;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IPaisDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<PaisDataModel>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<PaisDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<PaisDataModel?> GetByNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);

        Task<PaisDataModel?> GetByCodigoIsoAsync(
            string codigoIso,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<PaisDataModel> CreateAsync(
            PaisDataModel model,
            CancellationToken cancellationToken = default);

        Task<PaisDataModel> UpdateAsync(
            PaisDataModel model,
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

        Task<bool> ExistsByCodigoIsoAsync(
            string codigoIso,
            CancellationToken cancellationToken = default);

        Task<bool> TieneCiudadesAsociadasAsync(
            int idPais,
            CancellationToken cancellationToken = default);
    }
}