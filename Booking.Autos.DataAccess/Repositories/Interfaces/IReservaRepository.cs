using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IReservaRepository
    {
        // =========================
        // CONSULTAS BASE
        // =========================

        Task<IEnumerable<ReservaEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<ReservaEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<ReservaEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

        // =========================
        // FILTROS DE NEGOCIO
        // =========================

        Task<IEnumerable<ReservaEntity>> GetByClienteAsync(int idCliente, CancellationToken cancellationToken = default);

        Task<IEnumerable<ReservaEntity>> GetByVehiculoAsync(int idVehiculo, CancellationToken cancellationToken = default);

        Task<IEnumerable<ReservaEntity>> GetByEstadoAsync(string estado, CancellationToken cancellationToken = default);

        // =========================
        // DISPONIBILIDAD (CRÍTICO)
        // =========================

        Task<bool> IsVehiculoDisponibleAsync(
            int idVehiculo,
            DateTime inicio,
            DateTime fin,
            CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(ReservaEntity reserva, CancellationToken cancellationToken = default);

        Task UpdateAsync(ReservaEntity reserva, CancellationToken cancellationToken = default);

        // =========================
        // CASOS DE USO (DOMINIO)
        // =========================

        Task ConfirmarReservaAsync(int id, CancellationToken cancellationToken = default);

        Task CancelarReservaAsync(int id, string motivo, CancellationToken cancellationToken = default);

        // ⚠️ recomendado NO borrar reservas en sistemas reales
    }
}