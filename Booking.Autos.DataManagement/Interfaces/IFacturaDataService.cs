using Booking.Autos.DataManagement.Models;
using Booking.Autos.DataManagement.Models.Facturas;
using Microservicio.Clientes.DataManagement.Models;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IFacturaDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<DataPagedResult<FacturaDataModel>> GetAllAsync(
            FacturaFiltroDataModel filtro,
            CancellationToken cancellationToken = default);

        Task<FacturaDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<FacturaDataModel>> GetByClienteAsync(
            int idCliente,
            CancellationToken cancellationToken = default);

        Task<FacturaDataModel?> GetByReservaAsync(
            int idReserva,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<FacturaDataModel> CreateAsync(
            FacturaDataModel model,
            CancellationToken cancellationToken = default);

        // =========================
        // ACCIONES DE NEGOCIO
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