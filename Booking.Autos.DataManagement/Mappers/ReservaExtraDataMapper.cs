using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class ReservaExtraDataMapper
    {
        // 🔁 Entity → DataModel
        public static ReservaExtraDataModel ToDataModel(ReservaExtraEntity entity)
        {
            return new ReservaExtraDataModel
            {
                Id = entity.id_reserva_extra,
                Guid = entity.r_x_e_guid,

                IdReserva = entity.id_reserva,
                IdExtra = entity.id_extra,

                Cantidad = entity.r_x_e_cantidad,
                ValorUnitario = entity.r_x_e_valor_unitario,
                Subtotal = entity.r_x_e_subtotal,

                Estado = entity.r_x_e_estado,
                EsEliminado = entity.es_eliminado,

                FechaCreacion = entity.fecha_creacion,
                FechaActualizacion = entity.fecha_actualizacion,
                FechaEliminacion = entity.fecha_eliminacion
            };
        }

        // 🔁 DataModel → Entity
        public static ReservaExtraEntity ToEntity(ReservaExtraDataModel model)
        {
            return new ReservaExtraEntity
            {
                id_reserva_extra = model.Id,
                r_x_e_guid = model.Guid,

                id_reserva = model.IdReserva,
                id_extra = model.IdExtra,

                r_x_e_cantidad = model.Cantidad,
                r_x_e_valor_unitario = model.ValorUnitario,
                r_x_e_subtotal = model.Subtotal,

                r_x_e_estado = model.Estado,
                es_eliminado = model.EsEliminado,

                fecha_creacion = model.FechaCreacion,
                fecha_actualizacion = model.FechaActualizacion,
                fecha_eliminacion = model.FechaEliminacion
            };
        }

        // 🔥 Helper lista
        public static List<ReservaExtraDataModel> ToDataModelList(IEnumerable<ReservaExtraEntity> entities)
        {
            return entities.Select(ToDataModel).ToList();
        }
    }
}