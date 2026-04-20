using Booking.Autos.Business.DTOs.Catalogos.Marca;
using Booking.Autos.Business.DTOs.Marca;

namespace Booking.Autos.Business.Interfaces
{
    public interface IMarcaService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<MarcaResponse> CrearAsync(
            CrearMarcaRequest request,
            CancellationToken cancellationToken = default);

        Task<MarcaResponse> ActualizarAsync(
            ActualizarMarcaRequest request,
            CancellationToken cancellationToken = default);

        Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<MarcaResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<MarcaResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        // 🔍 búsqueda puntual
        Task<MarcaResponse?> ObtenerPorNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistePorNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);
    }
}