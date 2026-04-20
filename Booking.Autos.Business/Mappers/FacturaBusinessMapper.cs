using Booking.Autos.Business.DTOs.Factura;
using Booking.Autos.DataManagement.Models.Facturas;

namespace Booking.Autos.Business.Mappers
{
    public static class FacturaBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static FacturaDataModel ToDataModel(CrearFacturaRequest request)
        {
            return new FacturaDataModel
            {
                IdReserva = request.IdReserva,
                IdCliente = request.IdCliente,

                Descripcion = request.Descripcion,
                Origen = request.Origen,

                // 🔥 valores iniciales (se recalculan en service)
              

                // 🔥 estado inicial
                Estado = "PEN", // pendiente

                // 🔥 auditoría
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow,
                EsEliminado = false
            };
        }

        // =========================
        // ACTUALIZAR → DATAMODEL
        // =========================
        public static FacturaDataModel ToDataModel(ActualizarFacturaRequest request)
        {
            return new FacturaDataModel
            {
                Id = request.Id,

                Descripcion = request.Descripcion,
                

                FechaActualizacion = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static FacturaResponse ToResponse(FacturaDataModel model)
        {
            return new FacturaResponse
            {
                Id = model.Id,
                Guid = model.Guid,

                IdReserva = model.IdReserva,
                IdCliente = model.IdCliente,

                Descripcion = model.Descripcion,
                Origen = model.Origen,

                Subtotal = model.Subtotal,
                Iva = model.Iva,
                Total = model.Total,

                Estado = model.Estado,

                FechaAprobacion = model.FechaAprobacion,
                FechaAnulacion = model.FechaAnulacion,
                MotivoAnulacion = model.MotivoAnulacion
            };
        }

        // =========================
        // LISTA
        // =========================
        public static List<FacturaResponse> ToResponseList(IEnumerable<FacturaDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}