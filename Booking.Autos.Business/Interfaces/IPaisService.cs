using Booking.Autos.Business.DTOs.Catalogos.Pais;
using Booking.Autos.Business.DTOs.Pais;

namespace Booking.Autos.Business.Interfaces
{
    public interface IPaisService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<PaisResponse> CrearAsync(
            CrearPaisRequest request,
            CancellationToken cancellationToken = default);

        Task<PaisResponse> ActualizarAsync(
            ActualizarPaisRequest request,
            CancellationToken cancellationToken = default);

        Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<PaisResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<PaisResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        Task<PaisResponse?> ObtenerPorNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);

        // 🔥 CLAVE (ISO)
        Task<PaisResponse?> ObtenerPorCodigoIsoAsync(
            string codigoIso,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistePorNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);

        Task<bool> ExistePorCodigoIsoAsync(
            string codigoIso,
            CancellationToken cancellationToken = default);

        // 🔥 REGLA DE NEGOCIO
        Task<bool> TieneCiudadesAsociadasAsync(
            int idPais,
            CancellationToken cancellationToken = default);
    }
}