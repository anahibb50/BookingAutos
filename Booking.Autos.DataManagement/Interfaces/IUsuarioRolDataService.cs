using Booking.Autos.DataManagement.Models.UsuarioApp;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IUsuarioRolDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<UsuarioRolDataModel>> GetByUsuarioAsync(
            int idUsuario,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<UsuarioRolDataModel>> GetByRolAsync(
            int idRol,
            CancellationToken cancellationToken = default);

        Task<UsuarioRolDataModel?> GetAsync(
            int idUsuario,
            int idRol,
            CancellationToken cancellationToken = default);

        // 🔥 CLAVE PARA LOGIN
        Task<List<string>> GetRolesByUsuarioAsync(
            int idUsuario,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AssignAsync(
            UsuarioRolDataModel model,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
            UsuarioRolDataModel model,
            CancellationToken cancellationToken = default);

        Task RemoveAsync(
            int idUsuario,
            int idRol,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsAsync(
            int idUsuario,
            int idRol,
            CancellationToken cancellationToken = default);
    }
}