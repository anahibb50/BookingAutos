using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Models.Conductores;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IConductorDataService
    {
        Task<IReadOnlyList<ConductorDataModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ConductorDataModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ConductorDataModel?> GetByIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default);
        Task<ConductorDataModel?> GetByLicenciaAsync(string numeroLicencia, CancellationToken cancellationToken = default);
        Task<DataPagedResult<ConductorDataModel>> BuscarAsync(ConductorFiltroDataModel filtro, CancellationToken cancellationToken = default);
        Task<ConductorDataModel> CreateAsync(ConductorDataModel model, CancellationToken cancellationToken = default);
        Task<ConductorDataModel> UpdateAsync(ConductorDataModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default);
        Task<bool> ExistsByLicenciaAsync(string numeroLicencia, CancellationToken cancellationToken = default);
    }
}
