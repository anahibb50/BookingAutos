using Booking.Autos.Business.DTOs.Usuario;

namespace Booking.Autos.Business.Interfaces
{
    public interface IUsuarioAppService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<UsuarioResponse> CrearAsync(
            CrearUsuarioRequest request,
            string usuario,
            CancellationToken cancellationToken = default);

        Task<UsuarioResponse> ActualizarAsync(
            int id,
            CrearUsuarioRequest request,
            string usuario,
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
        // CONSULTAS
        // =========================

        Task<UsuarioResponse?> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<UsuarioResponse?> ObtenerPorUsernameAsync(
            string username,
            CancellationToken cancellationToken = default);

        Task<UsuarioResponse?> ObtenerPorCorreoAsync(
            string correo,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<UsuarioResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistePorUsernameAsync(
            string username,
            CancellationToken cancellationToken = default);

        Task<bool> ExistePorCorreoAsync(
            string correo,
            CancellationToken cancellationToken = default);
    }
}