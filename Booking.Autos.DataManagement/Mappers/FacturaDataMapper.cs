using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.Facturas;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class FacturaDataMapper
    {
        // 🔁 Entity → DataModel
        public static FacturaDataModel ToDataModel(FacturaEntity entity)
        {
            return new FacturaDataModel
            {
                Id = entity.id_factura,
                Guid = entity.factura_guid,

                IdReserva = entity.id_reserva,
                IdCliente = entity.id_cliente,

                Descripcion = entity.fac_descripcion,
                Origen = entity.origen_factura,

                Subtotal = entity.fac_subtotal,
                Iva = entity.fac_iva,
                Total = entity.fac_total,

                Estado = entity.fac_estado,

                FechaAprobacion = entity.fecha_aprobacion,
                FechaAnulacion = entity.fecha_anulacion,
                MotivoAnulacion = entity.motivo_anulacion,

                FechaCreacion = entity.fecha_creacion,
                FechaActualizacion = entity.fecha_actualizacion,
                FechaEliminacion = entity.fecha_eliminacion,
                EsEliminado = entity.es_eliminado
            };
        }

        // 🔁 DataModel → Entity
        public static FacturaEntity ToEntity(FacturaDataModel model)
        {
            return new FacturaEntity
            {
                id_factura = model.Id,
                factura_guid = model.Guid,

                id_reserva = model.IdReserva,
                id_cliente = model.IdCliente,

                fac_descripcion = model.Descripcion,
                origen_factura = model.Origen,

                fac_subtotal = model.Subtotal,
                fac_iva = model.Iva,
                fac_total = model.Total,

                fac_estado = model.Estado,

                fecha_aprobacion = model.FechaAprobacion,
                fecha_anulacion = model.FechaAnulacion,
                motivo_anulacion = model.MotivoAnulacion,

                fecha_creacion = model.FechaCreacion,
                fecha_actualizacion = model.FechaActualizacion,
                fecha_eliminacion = model.FechaEliminacion,
                es_eliminado = model.EsEliminado
            };
        }

        // 🔥 Helper lista
        public static List<FacturaDataModel> ToDataModelList(IEnumerable<FacturaEntity> entities)
        {
            return entities.Select(ToDataModel).ToList();
        }
    }
}