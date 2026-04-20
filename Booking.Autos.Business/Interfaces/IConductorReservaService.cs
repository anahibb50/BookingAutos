using Booking.Autos.Business.DTOs.ConductorReserva;

namespace Booking.Autos.Business.Interfaces
{
    public interface IConductorReservaService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<ConductorReservaResponse> AsignarAsync(
            AssignConductorRequest request,
            CancellationToken cancellationToken = default);

        Task<ConductorReservaResponse> ActualizarAsync(
            AssignConductorRequest request,
            CancellationToken cancellationToken = default);

        Task RemoverAsync(
            int idReserva,
            int idConductor,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<IReadOnlyList<ConductorReservaResponse>> ObtenerPorReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ConductorReservaResponse>> ObtenerPorConductorAsync(
            int idConductor,
            CancellationToken cancellationToken = default);

        Task<ConductorReservaResponse?> ObtenerAsync(
            int idReserva,
            int idConductor,
            CancellationToken cancellationToken = default);

        // 🔥 CLAVE DE NEGOCIO
        Task<ConductorReservaResponse?> ObtenerPrincipalPorReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExisteAsync(
            int idReserva,
            int idConductor,
            CancellationToken cancellationToken = default);
    }
}