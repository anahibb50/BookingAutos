using Booking.Autos.DataManagement.Models.UsuarioApp;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IUsuarioAppDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<UsuarioAppDataModel>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<UsuarioAppDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<UsuarioAppDataModel?> GetByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default);

        Task<UsuarioAppDataModel?> GetByCorreoAsync(
            string correo,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<UsuarioAppDataModel> CreateAsync(
            UsuarioAppDataModel model,
            string passwordPlano,
            CancellationToken cancellationToken = default);

        Task<UsuarioAppDataModel> UpdateAsync(
            UsuarioAppDataModel model,
            CancellationToken cancellationToken = default);

        // =========================
        // ESTADO
        // =========================

        Task<bool> ActivarAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<bool> DesactivarAsync(
            int id,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsByCorreoAsync(
            string correo,
            CancellationToken cancellationToken = default);
    }
}