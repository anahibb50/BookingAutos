using Booking.Autos.Business.DTOs.Cliente;
using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Models;

namespace Booking.Autos.Business.Interfaces
{
    public interface IClienteService
    {
        // =========================
        // ESCRITURA
        // =========================

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

        // =========================
        // CONSULTAS
        // =========================

        Task<ClienteResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<ClienteResponse?> ObtenerPorIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ClienteResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        // 🔥 PAGINACIÓN
        Task<DataPagedResult<ClienteResponse>> BuscarAsync(
            ClienteFiltroRequest request,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistePorIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default);
    }
}