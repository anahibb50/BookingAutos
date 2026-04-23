using Booking.Autos.Business.DTOs.Vehiculo;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.Business.Interfaces
{
    public interface IVehiculoService
    {
        Task<VehiculoResponse> CrearAsync(
            CrearVehiculoRequest request,
            CancellationToken cancellationToken = default);

        Task<VehiculoResponse> ActualizarAsync(
            ActualizarVehiculoRequest request,
            CancellationToken cancellationToken = default);

        Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default);

        Task<VehiculoResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<VehiculoResponse?> ObtenerPorPlacaAsync(
            string placa,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<VehiculoResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        Task<DataPagedResult<VehiculoResponse>> BuscarAsync(
            VehiculoFiltroRequest request,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<VehiculoResponse>> ObtenerPorMarcaAsync(
            int idMarca,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<VehiculoResponse>> ObtenerPorCategoriaAsync(
            int idCategoria,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<VehiculoResponse>> ListarDisponiblesAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<VehiculoResponse>> ObtenerPorRangoPrecioAsync(
            decimal min,
            decimal max,
            CancellationToken cancellationToken = default);

        Task<bool> ActualizarKilometrajeAsync(
            int id,
            int nuevoKilometraje,
            CancellationToken cancellationToken = default);

        Task<bool> ActualizarEstadoAsync(
            int id,
            string estado,
            CancellationToken cancellationToken = default);

        Task<bool> ExistePorPlacaAsync(
            string placa,
            CancellationToken cancellationToken = default);
    }
}
