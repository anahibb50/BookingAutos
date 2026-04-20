using Booking.Autos.Business.DTOs.ReservaExtra;

namespace Booking.Autos.Business.Interfaces
{
    public interface IReservaExtraService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<ReservaExtraResponse> AgregarAsync(
            AddExtraRequest request,
            CancellationToken cancellationToken = default);

        Task<ReservaExtraResponse> ActualizarAsync(
            AddExtraRequest request,
            CancellationToken cancellationToken = default);

        Task RemoverAsync(
            int id,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<ReservaExtraResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ReservaExtraResponse>> ObtenerPorReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ReservaExtraResponse>> ObtenerPorExtraAsync(
            int idExtra,
            CancellationToken cancellationToken = default);

        // =========================
        // CÁLCULO 🔥🔥🔥
        // =========================

        Task<decimal> ObtenerSubtotalPorReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);
    }
}