using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Reservas;
using Booking.Autos.DataManagement.Mappers;

namespace Booking.Autos.DataManagement.Services
{
    public class ConductorReservaDataService : IConductorReservaDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConductorReservaDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<ConductorReservaDataModel>> GetByReservaAsync(
            int idReserva,
            CancellationToken ct = default)
        {
            var entities = await _unitOfWork.ConductoresReservas
                .GetByReservaIdAsync(idReserva, ct);

            return entities.Select(ConductorReservaDataMapper.ToDataModel);
        }

        public async Task<IEnumerable<ConductorReservaDataModel>> GetByConductorAsync(
            int idConductor,
            CancellationToken ct = default)
        {
            var entities = await _unitOfWork.ConductoresReservas
                .GetByConductorIdAsync(idConductor, ct);

            return entities.Select(ConductorReservaDataMapper.ToDataModel);
        }

        public async Task<ConductorReservaDataModel?> GetAsync(
            int idReserva,
            int idConductor,
            CancellationToken ct = default)
        {
            var entity = await _unitOfWork.ConductoresReservas
                .GetByIdsAsync(idReserva, idConductor, ct);

            return entity == null
                ? null
                : ConductorReservaDataMapper.ToDataModel(entity);
        }

        public async Task<ConductorReservaDataModel?> GetPrincipalByReservaAsync(
            int idReserva,
            CancellationToken ct = default)
        {
            var entity = await _unitOfWork.ConductoresReservas
                .GetConductorPrincipalByReservaIdAsync(idReserva, ct);

            return entity == null
                ? null
                : ConductorReservaDataMapper.ToDataModel(entity);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AssignAsync(
            ConductorReservaDataModel model,
            CancellationToken ct = default)
        {
            var entity = ConductorReservaDataMapper.ToEntity(model);

            await _unitOfWork.ConductoresReservas.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(
            ConductorReservaDataModel model,
            CancellationToken ct = default)
        {
            var entity = ConductorReservaDataMapper.ToEntity(model);

            await _unitOfWork.ConductoresReservas.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task RemoveAsync(
            int idReserva,
            int idConductor,
            CancellationToken ct = default)
        {
            await _unitOfWork.ConductoresReservas
                .DeleteAsync(idReserva, idConductor, ct);

            await _unitOfWork.SaveChangesAsync(ct);
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsAsync(
            int idReserva,
            int idConductor,
            CancellationToken ct = default)
        {
            return await _unitOfWork.ConductoresReservas
                .IsConductorAssignedToReservaAsync(idReserva, idConductor, ct);
        }
    }
}