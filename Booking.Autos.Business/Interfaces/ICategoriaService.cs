using Booking.Autos.Business.DTOs.Catalogos.Categoria;

namespace Booking.Autos.Business.Interfaces
{
    public interface ICategoriaService
    {
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

        Task<CategoriaResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<CategoriaResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        Task<bool> ExistePorNombreAsync(
            string nombre,
            CancellationToken cancellationToken = default);
    }
}
