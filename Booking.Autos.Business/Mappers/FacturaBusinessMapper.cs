using Booking.Autos.Business.DTOs.Factura;
using Booking.Autos.DataManagement.Models.Facturas;

namespace Booking.Autos.Business.Mappers
{
    public static class FacturaBusinessMapper
    {
        public static FacturaDataModel ToDataModel(CrearFacturaRequest request)
        {
            return new FacturaDataModel
            {
                IdReserva = request.IdReserva,
                Descripcion = request.Descripcion,
                Origen = request.Origen,
                Estado = "ABI",
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow,
                EsEliminado = false
            };
        }

        public static FacturaDataModel ToDataModel(ActualizarFacturaRequest request)
        {
            return new FacturaDataModel
            {
                Id = request.Id,
                Descripcion = request.Descripcion,
                FechaActualizacion = DateTime.UtcNow
            };
        }

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

        public static List<FacturaResponse> ToResponseList(IEnumerable<FacturaDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}
