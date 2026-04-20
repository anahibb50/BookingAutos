using Booking.Autos.Business.DTOs.Localizacion;

namespace Booking.Autos.Business.Interfaces
{
    public interface ILocalizacionService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<LocalizacionResponse> CrearAsync(
            CrearLocalizacionRequest request,
            CancellationToken cancellationToken = default);

        Task<LocalizacionResponse> ActualizarAsync(
            ActualizarLocalizacionRequest request,
            CancellationToken cancellationToken = default);

        // 🔥 NO ES DELETE → ES INHABILITAR
        Task InhabilitarAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<LocalizacionResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<LocalizacionResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<LocalizacionResponse>> ObtenerPorCiudadAsync(
            int idCiudad,
            CancellationToken cancellationToken = default);

        // 🔥 búsqueda puntual
        Task<LocalizacionResponse?> ObtenerPorNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistePorNombreEnCiudadAsync(
            string nombre,
            int idCiudad,
            CancellationToken cancellationToken = default);
    }
}