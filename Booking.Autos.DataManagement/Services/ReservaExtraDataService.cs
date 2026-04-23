using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Mappers;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.DataManagement.Services
{
    public class ReservaExtraDataService : IReservaExtraDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservaExtraDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ReservaExtraDataModel>> GetByReservaAsync(int idReserva, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.ReservasExtras.GetByReservaAsync(idReserva, ct);
            return ReservaExtraDataMapper.ToDataModelList(entities);
        }

        public async Task<IEnumerable<ReservaExtraDataModel>> GetByExtraAsync(int idExtra, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.ReservasExtras.GetByExtraAsync(idExtra, ct);
            return ReservaExtraDataMapper.ToDataModelList(entities);
        }

        public async Task<ReservaExtraDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException("Agrega GetByIdAsync en el repository o usa GUID");
        }

        public async Task<ReservaExtraDataModel> AddAsync(ReservaExtraDataModel model, CancellationToken ct = default)
        {
            var extra = await _unitOfWork.Extras.GetByIdAsync(model.IdExtra, ct);

            if (extra == null)
                throw new Exception("Extra no encontrado");

            var entity = ReservaExtraDataMapper.ToEntity(model);
            entity.r_x_e_valor_unitario = extra.valor_fijo;
            entity.r_x_e_subtotal = model.Cantidad * extra.valor_fijo;

            await _unitOfWork.ReservasExtras.AddAsync(entity, ct);

            return ReservaExtraDataMapper.ToDataModel(entity);
        }

        public async Task<ReservaExtraDataModel> UpdateAsync(ReservaExtraDataModel model, CancellationToken ct = default)
        {
            var extra = await _unitOfWork.Extras.GetByIdAsync(model.IdExtra, ct);
            if (extra == null)
                throw new Exception("Extra no encontrado");

            var entity = ReservaExtraDataMapper.ToEntity(model);
            entity.r_x_e_valor_unitario = extra.valor_fijo;
            entity.r_x_e_subtotal = model.Cantidad * extra.valor_fijo;
            entity.r_x_e_estado = string.IsNullOrWhiteSpace(model.Estado) ? "PEN" : model.Estado;

            await _unitOfWork.ReservasExtras.UpdateAsync(entity, ct);

            return ReservaExtraDataMapper.ToDataModel(entity);
        }

        public async Task<bool> RemoveAsync(int id, CancellationToken ct = default)
        {
            await _unitOfWork.ReservasExtras.RemoveAsync(id, ct);
            return true;
        }

        public async Task<decimal> GetSubtotalByReservaAsync(int idReserva, CancellationToken ct = default)
        {
            return await _unitOfWork.ReservasExtras.GetSubtotalByReservaAsync(idReserva, ct);
        }
    }
}
