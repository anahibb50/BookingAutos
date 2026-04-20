using Booking.Autos.Business.DTOs.Extra;

namespace Booking.Autos.Business.Interfaces
{
    public interface IExtraService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<ExtraResponse> CrearAsync(
            CrearExtraRequest request,
            CancellationToken cancellationToken = default);

        Task<ExtraResponse> ActualizarAsync(
            ActualizarExtraRequest request,
            CancellationToken cancellationToken = default);

        Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<ExtraResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ExtraResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        // 🔥 SOLO ACTIVOS
        Task<IReadOnlyList<ExtraResponse>> ListarActivosAsync(
            CancellationToken cancellationToken = default);

        // 🔍 BÚSQUEDA
        Task<IReadOnlyList<ExtraResponse>> ObtenerPorNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);

        // =========================
        // OPERACIÓN ESPECIAL 🔥
        // =========================

        Task<bool> ActualizarPrecioAsync(
            int id,
            decimal nuevoPrecio,
            CancellationToken cancellationToken = default);
    }
}