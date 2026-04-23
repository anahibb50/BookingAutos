using Booking.Autos.Business.DTOs.Cliente;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.Business.Interfaces
{
    public interface IClienteService
    {
        Task<ClienteResponse> CrearAsync(
            CrearClienteRequest request,
            CancellationToken cancellationToken = default);

        Task<ClienteResponse> ActualizarAsync(
            ActualizarClienteRequest request,
            CancellationToken cancellationToken = default);

        Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default);

        Task<ClienteResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<ClienteResponse?> ObtenerPorIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ClienteResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        Task<DataPagedResult<ClienteResponse>> BuscarAsync(
            ClienteFiltroRequest request,
            CancellationToken cancellationToken = default);

        Task<bool> ExistePorIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default);
    }
}
