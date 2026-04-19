using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class ReservaDataMapper
    {
        // 🔁 Entity → DataModel
        public static ReservaDataModel ToDataModel(ReservaEntity entity)
        {
            return new ReservaDataModel
            {
                Id = entity.id_reserva,
                Guid = entity.guid_reserva,
                Codigo = entity.codigo_reserva,

                IdCliente = entity.id_cliente,
                IdVehiculo = entity.id_vehiculo,
                IdLocalizacionRecogida = entity.id_localizacion_recogida,
                IdLocalizacionEntrega = entity.id_localizacion_entrega,

                FechaReservaUtc = entity.fecha_reserva_utc,
                FechaInicio = entity.fecha_inicio,
                FechaFin = entity.fecha_fin,

                HoraInicio = entity.hora_inicio,
                HoraFin = entity.hora_fin,

                CantidadDias = entity.cantidad_dias,

                Subtotal = entity.subtotal_reserva,
                Iva = entity.valor_iva,
                Total = entity.total_reserva,

                Descripcion = entity.descripcion_reserva,
                OrigenCanal = entity.origen_canal_reserva,

                Estado = entity.estado_reserva,

                FechaConfirmacionUtc = entity.fecha_confirmacion_utc,
                FechaCancelacionUtc = entity.fecha_cancelacion_utc,
                MotivoCancelacion = entity.motivo_cancelacion,

                CreadoPorUsuario = entity.creado_por_usuario,
                ModificadoPorUsuario = entity.modificado_por_usuario,
                FechaModificacionUtc = entity.fecha_modificacion_utc,
                ModificacionIp = entity.modificacion_ip,
                ServicioOrigen = entity.servicio_origen,

                EsEliminado = entity.es_eliminado,
                FechaEliminacion = entity.fecha_eliminacion
            };
        }

        // 🔁 DataModel → Entity
        public static ReservaEntity ToEntity(ReservaDataModel model)
        {
            return new ReservaEntity
            {
                id_reserva = model.Id,
                guid_reserva = model.Guid,
                codigo_reserva = model.Codigo,

                id_cliente = model.IdCliente,
                id_vehiculo = model.IdVehiculo,
                id_localizacion_recogida = model.IdLocalizacionRecogida,
                id_localizacion_entrega = model.IdLocalizacionEntrega,

                fecha_reserva_utc = model.FechaReservaUtc,
                fecha_inicio = model.FechaInicio,
                fecha_fin = model.FechaFin,

                hora_inicio = model.HoraInicio,
                hora_fin = model.HoraFin,

                cantidad_dias = model.CantidadDias,

                subtotal_reserva = model.Subtotal,
                valor_iva = model.Iva,
                total_reserva = model.Total,

                descripcion_reserva = model.Descripcion,
                origen_canal_reserva = model.OrigenCanal,

                estado_reserva = model.Estado,

                fecha_confirmacion_utc = model.FechaConfirmacionUtc,
                fecha_cancelacion_utc = model.FechaCancelacionUtc,
                motivo_cancelacion = model.MotivoCancelacion,

                creado_por_usuario = model.CreadoPorUsuario,
                modificado_por_usuario = model.ModificadoPorUsuario,
                fecha_modificacion_utc = model.FechaModificacionUtc,
                modificacion_ip = model.ModificacionIp,
                servicio_origen = model.ServicioOrigen,

                es_eliminado = model.EsEliminado,
                fecha_eliminacion = model.FechaEliminacion
            };
        }

        // 🔥 Helper lista
        public static List<ReservaDataModel> ToDataModelList(IEnumerable<ReservaEntity> entities)
        {
            return entities.Select(ToDataModel).ToList();
        }
    }
}