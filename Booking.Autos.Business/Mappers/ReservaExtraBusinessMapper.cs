using Booking.Autos.Business.DTOs.ReservaExtra;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.Business.Mappers
{
    public static class ReservaExtraBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static ReservaExtraDataModel ToDataModel(CrearReservaExtraDetalleRequest request)
        {
            return new ReservaExtraDataModel
            {
                // IdReserva se asigna en el service

                IdExtra = request.IdExtra,
                Cantidad = request.Cantidad,

                // 🔥 valores calculados en service
                // ValorUnitario
                // Subtotal

                Estado = "PEN",
                EsEliminado = false,

                FechaCreacion = DateTime.UtcNow
            };
        }

        // =========================
        // ACTUALIZAR → DATAMODEL (MISMO ESTILO QUE CIUDAD)
        // =========================
        public static ReservaExtraDataModel ToDataModel(ActualizarReservaExtraDetalleRequest request)
        {
            return new ReservaExtraDataModel
            {
                Id = request.Id!.Value,
                IdExtra = request.IdExtra,
                Cantidad = request.Cantidad,

                FechaActualizacion = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static ReservaExtraDetalleResponse ToResponse(ReservaExtraDataModel model)
        {
            return new ReservaExtraDetalleResponse
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
        public static List<ReservaExtraDetalleResponse> ToResponseList(IEnumerable<ReservaExtraDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}
