using Booking.Autos.Business.DTOs.ReservaExtra;

namespace Booking.Autos.Business.Interfaces
{
    public interface IReservaExtraService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<ReservaExtraDetalleResponse?> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ReservaExtraDetalleResponse>> ObtenerPorReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ReservaExtraDetalleResponse>> ObtenerPorExtraAsync(
            int idExtra,
            CancellationToken cancellationToken = default);

        // =========================
        // CÁLCULO 🔥
        // =========================

        Task<decimal> ObtenerSubtotalPorReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);
    }
}