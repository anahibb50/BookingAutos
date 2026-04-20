using Booking.Autos.Business.DTOs.Catalogos.Categoria;

using Booking.Autos.DataManagement.Models;

namespace Booking.Autos.Business.Interfaces
{
    public interface ICategoriaService
    {
        // =========================
        // ESCRITURA
        // =========================

        Task<CategoriaResponse> CrearAsync(
            CrearCategoriaRequest request,
            CancellationToken cancellationToken = default);

        Task<CategoriaResponse> ActualizarAsync(
            ActualizarCategoriaRequest request,
            CancellationToken cancellationToken = default);

        Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<CategoriaResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<CategoriaResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistePorNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);
    }
}