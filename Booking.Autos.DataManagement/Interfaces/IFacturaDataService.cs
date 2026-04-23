using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Models.Facturas;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IFacturaDataService
    {
        Task<IReadOnlyList<FacturaDataModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<FacturaDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<FacturaDataModel>> GetByClienteAsync(int idCliente, CancellationToken cancellationToken = default);
        Task<FacturaDataModel?> GetByReservaAsync(int idReserva, CancellationToken cancellationToken = default);
        Task<DataPagedResult<FacturaDataModel>> BuscarAsync(FacturaFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<FacturaDataModel> CreateAsync(FacturaDataModel model, CancellationToken cancellationToken = default);
        Task<FacturaDataModel> UpdateAsync(FacturaDataModel model, CancellationToken cancellationToken = default);
        Task<bool> AprobarAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> AnularAsync(int id, string motivo, CancellationToken cancellationToken = default);
    }
}
