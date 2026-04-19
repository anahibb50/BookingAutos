using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.Localizaciones;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class LocalizacionDataMapper
    {
        // 🔁 Entity → DataModel
        public static LocalizacionDataModel ToDataModel(LocalizacionEntity entity)
        {
            return new LocalizacionDataModel
            {
                Id = entity.id_localizacion,
                Guid = entity.localizacion_guid,

                Codigo = entity.codigo_localizacion,
                Nombre = entity.nombre_localizacion,

                IdCiudad = entity.id_ciudad,

                Direccion = entity.direccion_localizacion,
                Telefono = entity.telefono_contacto,
                Correo = entity.correo_contacto,
                HorarioAtencion = entity.horario_atencion,
                ZonaHoraria = entity.zona_horaria,

                Estado = entity.estado_localizacion,
                EsEliminado = entity.es_eliminado,

                FechaRegistroUtc = entity.fecha_registro_utc,
                CreadoPorUsuario = entity.creado_por_usuario,

                ModificadoPorUsuario = entity.modificado_por_usuario,
                FechaModificacionUtc = entity.fecha_modificacion_utc,
                ModificadoDesdeIp = entity.modificado_desde_ip,

                FechaInhabilitacionUtc = entity.fecha_inhabilitacion_utc,
                MotivoInhabilitacion = entity.motivo_inhabilitacion,

                OrigenRegistro = entity.origen_registro,

                RowVersion = entity.row_version
            };
        }

        // 🔁 DataModel → Entity
        public static LocalizacionEntity ToEntity(LocalizacionDataModel model)
        {
            return new LocalizacionEntity
            {
                id_localizacion = model.Id,
                localizacion_guid = model.Guid,

                codigo_localizacion = model.Codigo,
                nombre_localizacion = model.Nombre,

                id_ciudad = model.IdCiudad,

                direccion_localizacion = model.Direccion,
                telefono_contacto = model.Telefono,
                correo_contacto = model.Correo,
                horario_atencion = model.HorarioAtencion,
                zona_horaria = model.ZonaHoraria,

                estado_localizacion = model.Estado,
                es_eliminado = model.EsEliminado,

                fecha_registro_utc = model.FechaRegistroUtc,
                creado_por_usuario = model.CreadoPorUsuario,

                modificado_por_usuario = model.ModificadoPorUsuario,
                fecha_modificacion_utc = model.FechaModificacionUtc,
                modificado_desde_ip = model.ModificadoDesdeIp,

                fecha_inhabilitacion_utc = model.FechaInhabilitacionUtc,
                motivo_inhabilitacion = model.MotivoInhabilitacion,

                origen_registro = model.OrigenRegistro,

                row_version = model.RowVersion
            };
        }

        // 🔥 Helper para listas
        public static List<LocalizacionDataModel> ToDataModelList(IEnumerable<LocalizacionEntity> entities)
        {
            return entities.Select(ToDataModel).ToList();
        }
    }
}