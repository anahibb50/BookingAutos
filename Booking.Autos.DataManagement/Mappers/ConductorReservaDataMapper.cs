using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class ConductorReservaDataMapper
    {
        // 🔁 Entity → DataModel
        public static ConductorReservaDataModel ToDataModel(ConductorReservaEntity entity)
        {
            return new ConductorReservaDataModel
            {
                IdReserva = entity.id_reserva,
                IdConductor = entity.id_conductor,

                Rol = entity.rol_conductor,
                EsPrincipal = entity.es_principal,

                FechaAsignacionUtc = entity.fecha_asignacion_utc,
                FechaDesasignacionUtc = entity.fecha_desasignacion_utc,

                Estado = entity.estado_asignacion,
                Observaciones = entity.observaciones,

                CreadoPorUsuario = entity.creado_por_usuario,
                FechaRegistroUtc = entity.fecha_registro_utc,

                ModificadoPorUsuario = entity.modificado_por_usuario,
                FechaModificacionUtc = entity.fecha_modificacion_utc,
                ModificacionIp = entity.modificacion_ip,
                ServicioOrigen = entity.servicio_origen,

                FechaEliminacion = entity.fecha_eliminacion
            };
        }

        // 🔁 DataModel → Entity
        public static ConductorReservaEntity ToEntity(ConductorReservaDataModel model)
        {
            return new ConductorReservaEntity
            {
                id_reserva = model.IdReserva,
                id_conductor = model.IdConductor,

                rol_conductor = model.Rol,
                es_principal = model.EsPrincipal,

                fecha_asignacion_utc = model.FechaAsignacionUtc,
                fecha_desasignacion_utc = model.FechaDesasignacionUtc,

                estado_asignacion = model.Estado,
                observaciones = model.Observaciones,

                creado_por_usuario = model.CreadoPorUsuario,
                fecha_registro_utc = model.FechaRegistroUtc,

                modificado_por_usuario = model.ModificadoPorUsuario,
                fecha_modificacion_utc = model.FechaModificacionUtc,
                modificacion_ip = model.ModificacionIp,
                servicio_origen = model.ServicioOrigen,

                fecha_eliminacion = model.FechaEliminacion
            };
        }
    }
}