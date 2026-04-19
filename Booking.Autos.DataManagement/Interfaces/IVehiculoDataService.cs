using Booking.Autos.DataManagement.Models;
using Booking.Autos.DataManagement.Models.Vehiculos;
using Microservicio.Clientes.DataManagement.Models;

namespace Booking.Autos.DataManagement.Interfaces
{
    public interface IVehiculoDataService
    {
        // =========================
        // CONSULTAS (CATÁLOGO)
        // =========================

        Task<DataPagedResult<VehiculoDataModel>> GetAllAsync(
            VehiculoFiltroDataModel filtro,
            CancellationToken cancellationToken = default);

        Task<VehiculoDataModel?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<VehiculoDataModel?> GetByPlacaAsync(
            string placa,
            CancellationToken cancellationToken = default);

        // =========================
        // FILTROS ESPECÍFICOS
        // =========================

        Task<IEnumerable<VehiculoDataModel>> GetByMarcaAsync(
            int idMarca,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<VehiculoDataModel>> GetByCategoriaAsync(
            int idCategoria,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<VehiculoDataModel>> GetDisponiblesAsync(
            CancellationToken cancellationToken = default);

        Task<IEnumerable<VehiculoDataModel>> GetByRangoPrecioAsync(
            decimal min,
            decimal max,
            CancellationToken cancellationToken = default);

        // =========================
        // DISPONIBILIDAD 🔥
        // =========================

        Task<bool> IsDisponibleAsync(
            int idVehiculo,
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task<VehiculoDataModel> CreateAsync(
            VehiculoDataModel model,
            CancellationToken cancellationToken = default);

        Task<VehiculoDataModel> UpdateAsync(
            VehiculoDataModel model,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);

        // =========================
        // OPERACIONES ESPECIALES
        // =========================

        Task<bool> UpdateKilometrajeAsync(
            int id,
            int nuevoKilometraje,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateEstadoAsync(
            int id,
            string estado,
            CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByPlacaAsync(
            string placa,
            CancellationToken cancellationToken = default);
    }
}