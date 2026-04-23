using Booking.Autos.Business.DTOs.Factura;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.Business.Interfaces
{
    public interface IFacturaService
    {
        Task<FacturaResponse> CrearAsync(
            CrearFacturaRequest request,
            CancellationToken cancellationToken = default);

        Task<FacturaResponse> ActualizarAsync(
            ActualizarFacturaRequest request,
            CancellationToken cancellationToken = default);

        Task<FacturaResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<FacturaResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<FacturaResponse>> ObtenerPorClienteAsync(
            int idCliente,
            CancellationToken cancellationToken = default);

        Task<FacturaResponse?> ObtenerPorReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);

        Task<DataPagedResult<FacturaResponse>> BuscarAsync(
            FacturaFiltroRequest request,
            CancellationToken cancellationToken = default);

        Task<bool> AprobarAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<bool> AnularAsync(
            int id,
            string motivo,
            CancellationToken cancellationToken = default);
    }
}
