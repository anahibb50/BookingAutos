using Booking.Autos.DataManagement.Models.Reservas;

public interface IConductorReservaDataService
{
    // =========================
    // CONSULTAS
    // =========================

    Task<IEnumerable<ConductorReservaDataModel>> GetByReservaAsync(
        int idReserva,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<ConductorReservaDataModel>> GetByConductorAsync(
        int idConductor,
        CancellationToken cancellationToken = default);

    Task<ConductorReservaDataModel?> GetAsync(
        int idReserva,
        int idConductor,
        CancellationToken cancellationToken = default);

    Task<ConductorReservaDataModel?> GetPrincipalByReservaAsync(
        int idReserva,
        CancellationToken cancellationToken = default);

    // =========================
    // ESCRITURA
    // =========================

    Task<ConductorReservaDataModel> CreateAsync(
        ConductorReservaDataModel model,
        CancellationToken cancellationToken = default);

    Task<ConductorReservaDataModel> UpdateAsync(
        ConductorReservaDataModel model,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        int idReserva,
        int idConductor,
        CancellationToken cancellationToken = default);

    // =========================
    // VALIDACIONES
    // =========================

    Task<bool> ExistsAsync(
        int idReserva,
        int idConductor,
        CancellationToken cancellationToken = default);
}