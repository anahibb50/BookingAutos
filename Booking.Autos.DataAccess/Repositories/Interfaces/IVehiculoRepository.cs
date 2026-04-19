using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IVehiculoRepository
    {
        // =========================
        // GESTIÓN DE FLOTA
        // =========================

        Task<IEnumerable<VehiculoEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<VehiculoEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<VehiculoEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

        // =========================
        // BÚSQUEDAS DE NEGOCIO
        // =========================

        Task<VehiculoEntity?> GetByPlacaAsync(string placa, CancellationToken cancellationToken = default);

        Task<IEnumerable<VehiculoEntity>> GetByMarcaIdAsync(int idMarca, CancellationToken cancellationToken = default);

        Task<IEnumerable<VehiculoEntity>> GetByCategoriaIdAsync(int idCategoria, CancellationToken cancellationToken = default);

        // =========================
        // OPERATIVO / DISPONIBILIDAD
        // =========================

        Task<IEnumerable<VehiculoEntity>> GetDisponiblesAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<VehiculoEntity>> GetByRangoPrecioAsync(decimal min, decimal max, CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(VehiculoEntity vehiculo, CancellationToken cancellationToken = default);

        Task UpdateAsync(VehiculoEntity vehiculo, CancellationToken cancellationToken = default);

        // =========================
        // OPERACIONES ESPECIALES
        // =========================

        Task UpdateKilometrajeAsync(int id, int nuevoKilometraje, CancellationToken cancellationToken = default);

        Task UpdateEstadoAsync(int id, string nuevoEstado, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);

        // =========================
        // VALIDACIONES
        // =========================

        Task<bool> ExistsByPlacaAsync(string placa, CancellationToken cancellationToken = default);
    }
}