using Booking.Autos.DataManagement.Models.UsuarioApp;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IRolDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<RolDataModel>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<RolDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<RolDataModel?> GetByNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<RolDataModel> CreateAsync(
            RolDataModel model,
            CancellationToken cancellationToken = default);

        Task<RolDataModel> UpdateAsync(
            RolDataModel model,
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