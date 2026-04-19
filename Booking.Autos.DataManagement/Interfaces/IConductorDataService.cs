using Booking.Autos.DataManagement.Models;
using Booking.Autos.DataManagement.Models.Conductores;
using Microservicio.Clientes.DataManagement.Models;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IConductorDataService
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IReadOnlyList<ConductorDataModel>> GetAllAsync(
            CancellationToken cancellationToken = default);


        Task<ConductorDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<ConductorDataModel?> GetByIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default);

        Task<ConductorDataModel?> GetByLicenciaAsync(
            string numeroLicencia,
            CancellationToken cancellationToken = default);

        Task<DataPagedResult<ConductorDataModel>> BuscarAsync(
            ConductorFiltroDataModel filtro,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<ConductorDataModel> CreateAsync(
            ConductorDataModel model,
            CancellationToken cancellationToken = default);

        Task<ConductorDataModel> UpdateAsync(
            ConductorDataModel model,
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

        Task<bool> ExistsByLicenciaAsync(
            string numeroLicencia,
            CancellationToken cancellationToken = default);
    }
}