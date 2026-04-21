using Booking.Autos.Business.DTOs.ReservaExtra;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Interfaces;

namespace Booking.Autos.Business.Services
{
    public class ReservaExtraService : IReservaExtraService
    {
        private readonly IReservaExtraDataService _dataService;

        public ReservaExtraService(IReservaExtraDataService dataService)
        {
            _dataService = dataService;
        }

        // =========================
        // OBTENER POR ID
        // =========================
        public async Task<ReservaExtraDetalleResponse?> ObtenerPorIdAsync(
            int id,
            CancellationToken ct = default)
        {
            if (id <= 0)
                throw new ValidationException(new List<string> { "Id inválido." });

            var model = await _dataService.GetByIdAsync(id, ct);

            return model is null
                ? null
                : ReservaExtraBusinessMapper.ToResponse(model);
        }

        // =========================
        // OBTENER POR RESERVA
        // =========================
        public async Task<IReadOnlyList<ReservaExtraDetalleResponse>> ObtenerPorReservaAsync(
            int idReserva,
            CancellationToken ct = default)
        {
            if (idReserva <= 0)
                throw new ValidationException(new List<string> { "IdReserva inválido." });

            var list = await _dataService.GetByReservaAsync(idReserva, ct);

            return ReservaExtraBusinessMapper.ToResponseList(list);
        }

        // =========================
        // OBTENER POR EXTRA
        // =========================
        public async Task<IReadOnlyList<ReservaExtraDetalleResponse>> ObtenerPorExtraAsync(
            int idExtra,
            CancellationToken ct = default)
        {
            if (idExtra <= 0)
                throw new ValidationException(new List<string> { "IdExtra inválido." });

            var list = await _dataService.GetByExtraAsync(idExtra, ct);

            return ReservaExtraBusinessMapper.ToResponseList(list);
        }

        // =========================
        // SUBTOTAL POR RESERVA 🔥
        // =========================
        public async Task<decimal> ObtenerSubtotalPorReservaAsync(
            int idReserva,
            CancellationToken ct = default)
        {
            if (idReserva <= 0)
                throw new ValidationException(new List<string> { "IdReserva inválido." });

            return await _dataService.GetSubtotalByReservaAsync(idReserva, ct);
        }
    }
}