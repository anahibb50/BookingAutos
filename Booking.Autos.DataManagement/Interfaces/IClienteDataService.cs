using Booking.Autos.DataManagement.Models;
using Booking.Autos.DataManagement.Models.Clientes;
using Microservicio.Clientes.DataManagement.Models;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IClienteDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IReadOnlyList<ClienteDataModel>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<ClienteDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<ClienteDataModel?> GetByIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default);

        Task<DataPagedResult<ClienteDataModel>> BuscarAsync(
            ClienteFiltroDataModel filtro,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<ClienteDataModel> CreateAsync(
            ClienteDataModel model,
            CancellationToken cancellationToken = default);

        Task<ClienteDataModel> UpdateAsync(
            ClienteDataModel model,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default);
    }
}