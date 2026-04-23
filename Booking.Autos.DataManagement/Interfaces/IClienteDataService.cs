using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Models.Clientes;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IClienteDataService
    {
        Task<IReadOnlyList<ClienteDataModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ClienteDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ClienteDataModel?> GetByIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default);
        Task<DataPagedResult<ClienteDataModel>> BuscarAsync(ClienteFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<ClienteDataModel> CreateAsync(ClienteDataModel model, CancellationToken cancellationToken = default);
        Task<ClienteDataModel> UpdateAsync(ClienteDataModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default);
    }
}
