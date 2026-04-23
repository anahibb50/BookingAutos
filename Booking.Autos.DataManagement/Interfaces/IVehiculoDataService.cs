using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Models.Vehiculos;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IVehiculoDataService
    {
        Task<IReadOnlyList<VehiculoDataModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<VehiculoDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<VehiculoDataModel?> GetByPlacaAsync(string placa, CancellationToken cancellationToken = default);
        Task<DataPagedResult<VehiculoDataModel>> BuscarAsync(VehiculoFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<IEnumerable<VehiculoDataModel>> GetByMarcaAsync(int idMarca, CancellationToken cancellationToken = default);
        Task<IEnumerable<VehiculoDataModel>> GetByCategoriaAsync(int idCategoria, CancellationToken cancellationToken = default);
        Task<IEnumerable<VehiculoDataModel>> GetDisponiblesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<VehiculoDataModel>> GetByRangoPrecioAsync(decimal min, decimal max, CancellationToken cancellationToken = default);
        Task<VehiculoDataModel> CreateAsync(VehiculoDataModel model, CancellationToken cancellationToken = default);
        Task<VehiculoDataModel> UpdateAsync(VehiculoDataModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> UpdateKilometrajeAsync(int id, int nuevoKilometraje, CancellationToken cancellationToken = default);
        Task<bool> UpdateEstadoAsync(int id, string estado, CancellationToken cancellationToken = default);
        Task<bool> ExistsByPlacaAsync(string placa, CancellationToken cancellationToken = default);
        Task<bool> ExisteMarcaAsync(int idMarca, CancellationToken ct = default);
        Task<bool> ExisteCategoriaAsync(int idCategoria, CancellationToken ct = default);
        Task<bool> ExisteLocalizacionAsync(int idLocalizacion, CancellationToken ct = default);
    }
}
