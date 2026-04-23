using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IReservaDataService
    {
        Task<IReadOnlyList<ReservaDataModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<DataPagedResult<ReservaDataModel>> BuscarAsync(ReservaFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<ReservaDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReservaDataModel>> GetByClienteAsync(int idCliente, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReservaDataModel>> GetByVehiculoAsync(int idVehiculo, CancellationToken cancellationToken = default);
        Task<bool> IsVehiculoDisponibleAsync(int idVehiculo, DateTime fechaInicio, DateTime fechaFin, CancellationToken cancellationToken = default);
        Task<ReservaDataModel> CreateAsync(ReservaDataModel model, CancellationToken cancellationToken = default);
        Task<ReservaDataModel> UpdateAsync(ReservaDataModel model, CancellationToken cancellationToken = default);
        Task<bool> ConfirmarAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> CancelarAsync(int id, string motivo, CancellationToken cancellationToken = default);
    }
}
