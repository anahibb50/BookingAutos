using Booking.Autos.Business.DTOs.Conductor;
using Booking.Autos.DataManagement.Models;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.Business.Interfaces
{
    public interface IConductorService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<ConductorResponse> CrearAsync(
            CrearConductorRequest request,
            CancellationToken cancellationToken = default);

        Task<ConductorResponse> ActualizarAsync(
            ActualizarConductorRequest request,
            CancellationToken cancellationToken = default);

        Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<ConductorResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<ConductorResponse?> ObtenerPorIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default);

        Task<ConductorResponse?> ObtenerPorLicenciaAsync(
            string numeroLicencia,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ConductorResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        // 🔥 PAGINADO (CLAVE)
        Task<DataPagedResult<ConductorResponse>> BuscarAsync(
            ConductorFiltroRequest request,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistePorIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default);

        Task<bool> ExistePorLicenciaAsync(
            string numeroLicencia,
            CancellationToken cancellationToken = default);
    }
}