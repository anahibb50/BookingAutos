using Booking.Autos.Business.DTOs.ConductorReserva;

namespace Booking.Autos.Business.Interfaces
{
    public interface IConductorReservaService
    {
        // =========================
        // ESCRITURA
        // =========================


        Task<ConductorReservaDetalleResponse> CrearAsync(
            int idReserva,
            CrearConductorReservaDetalleRequest request,
            CancellationToken ct = default);

        Task RemoverAsync(
            int idReserva,
            int idConductor,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<IReadOnlyList<ConductorReservaDetalleResponse>> ObtenerPorReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ConductorReservaDetalleResponse>> ObtenerPorConductorAsync(
            int idConductor,
            CancellationToken cancellationToken = default);

        Task<ConductorReservaDetalleResponse?> ObtenerAsync(
            int idReserva,
            int idConductor,
            CancellationToken cancellationToken = default);

        Task<ConductorReservaDetalleResponse?> ObtenerPrincipalPorReservaAsync(
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
