using Booking.Autos.Business.DTOs.Catalogos.Ciudad;
using Booking.Autos.Business.DTOs.Ciudad;

namespace Booking.Autos.Business.Interfaces
{
    public interface ICiudadService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<CiudadResponse> CrearAsync(
            CrearCiudadRequest request,
            CancellationToken cancellationToken = default);

        Task<CiudadResponse> ActualizarAsync(
            ActualizarCiudadRequest request,
            CancellationToken cancellationToken = default);

        Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<CiudadResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<CiudadResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        // 🔥 CLAVE (por país)
        Task<IReadOnlyList<CiudadResponse>> ObtenerPorPaisAsync(
            int idPais,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistePorNombreAsync(
            string nombre,
            int idPais,
            CancellationToken cancellationToken = default);
    }
}