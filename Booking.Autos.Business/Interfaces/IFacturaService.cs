using Booking.Autos.Business.DTOs.Factura;
using Booking.Autos.DataManagement.Models;
using Microservicio.Clientes.DataManagement.Models;

namespace Booking.Autos.Business.Interfaces
{
    public interface IFacturaService
    {
        // =========================
        // CREACIÓN
        // =========================

        Task<FacturaResponse> CrearAsync(
            CrearFacturaRequest request,
            CancellationToken cancellationToken = default);

        // =========================
        // CONSULTAS
        // =========================

        Task<FacturaResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<FacturaResponse>> ListarAsync(
            CancellationToken cancellationToken = default);

        // 🔥 POR CLIENTE
        Task<IReadOnlyList<FacturaResponse>> ObtenerPorClienteAsync(
            int idCliente,
            CancellationToken cancellationToken = default);

        // 🔥 POR RESERVA (CLAVE)
        Task<FacturaResponse?> ObtenerPorReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);

        // 🔥 PAGINACIÓN
        Task<DataPagedResult<FacturaResponse>> BuscarAsync(
            FacturaFiltroRequest request,
            CancellationToken cancellationToken = default);

        // =========================
        // ACCIONES DE NEGOCIO 🔥
        // =========================

        Task<bool> AprobarAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<bool> AnularAsync(
            int id,
            string motivo,
            CancellationToken cancellationToken = default);
    }
}