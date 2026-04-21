using Booking.Autos.Business.DTOs.UsuarioRol;

namespace Booking.Autos.Business.Interfaces
{
    public interface IUsuarioRolService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<UsuarioRolResponse> CrearAsync(
            CrearUsuarioRolRequest request,
            CancellationToken cancellationToken = default);

        Task<UsuarioRolResponse> ActualizarAsync(
            ActualizarUsuarioRolRequest request,
            CancellationToken cancellationToken = default);

        Task EliminarAsync(
            int id,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<UsuarioRolResponse?> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<UsuarioRolResponse>> ObtenerPorUsuarioAsync(
            int idUsuario,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<UsuarioRolResponse>> ObtenerPorRolAsync(
            int idRol,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<UsuarioRolResponse>> FiltrarAsync(
            UsuarioRolFiltroRequest request,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIÓN 🔥
        // =========================

        Task<bool> ExisteAsync(
            int idUsuario,
            int idRol,
            CancellationToken cancellationToken = default);
    }
}