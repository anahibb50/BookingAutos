using Booking.Autos.Business.DTOs.Rol;

namespace Booking.Autos.Business.Interfaces
{
    public interface IRolService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<RolResponse> CrearAsync(
            CrearRolRequest request,
            CancellationToken cancellationToken = default);

        Task<RolResponse> ActualizarAsync(
            ActualizarRolRequest request,
            CancellationToken cancellationToken = default);

        Task EliminarAsync(
            int id,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<RolResponse?> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<RolResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<RolResponse>> FiltrarAsync(
            RolFiltroRequest request,
            CancellationToken cancellationToken = default);
    }
}