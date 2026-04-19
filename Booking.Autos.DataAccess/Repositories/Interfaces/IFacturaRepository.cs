using Booking.Autos.DataAccess.Entities;

namespace Booking.Autos.DataAccess.Repositories.Interfaces
{
    public interface IFacturaRepository
    {
        // =========================
        // CONSULTAS
        // =========================

        Task<IEnumerable<FacturaEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<FacturaEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<FacturaEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default);

        Task<IEnumerable<FacturaEntity>> GetByClienteAsync(int idCliente, CancellationToken cancellationToken = default);

        Task<FacturaEntity?> GetByReservaAsync(int idReserva, CancellationToken cancellationToken = default);

        Task<IEnumerable<FacturaEntity>> GetByRangoFechasAsync(DateTime inicio, DateTime fin, CancellationToken cancellationToken = default);

        // =========================
        // CONSULTA POR ESTADO (TIPADO)
        // =========================

        Task<IEnumerable<FacturaEntity>> GetByEstadoAsync(string estado, CancellationToken cancellationToken = default);

        // =========================
        // ESCRITURA
        // =========================

        Task AddAsync(FacturaEntity factura, CancellationToken cancellationToken = default);

        // =========================
        // CASOS DE USO (NEGOCIO)
        // =========================

        Task AprobarAsync(int id, CancellationToken cancellationToken = default);

        Task AnularAsync(int id, string motivo, CancellationToken cancellationToken = default);
    }
}