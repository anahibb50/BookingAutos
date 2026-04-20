using Booking.Autos.Business.DTOs.ReservaExtra;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.Business.Mappers
{
    public static class ReservaExtraBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static ReservaExtraDataModel ToDataModel(AddExtraRequest request)
        {
            return new ReservaExtraDataModel
            {
                IdReserva = request.IdReserva,
                IdExtra = request.IdExtra,
                Cantidad = request.Cantidad,

                // 🔥 estos NO vienen del request
                // se llenan en el service:
                // ValorUnitario
                // Subtotal

                Estado = "ACT",
                EsEliminado = false,

                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static ReservaExtraResponse ToResponse(ReservaExtraDataModel model)
        {
            return new ReservaExtraResponse
            {
                Id = model.Id,
                Guid = model.Guid,

                IdReserva = model.IdReserva,
                IdExtra = model.IdExtra,

                Cantidad = model.Cantidad,
                ValorUnitario = model.ValorUnitario,
                Subtotal = model.Subtotal,

                Estado = model.Estado
            };
        }

        // =========================
        // LISTA
        // =========================
        public static List<ReservaExtraResponse> ToResponseList(IEnumerable<ReservaExtraDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}