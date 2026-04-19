using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.Extras;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class ExtraDataMapper
    {
        // 🔁 Entity → DataModel
        public static ExtraDataModel ToDataModel(ExtraEntity entity)
        {
            return new ExtraDataModel
            {
                Id = entity.id_extra,
                Guid = entity.extra_guid,

                Codigo = entity.codigo_extra,
                Nombre = entity.nombre_extra,
                Descripcion = entity.descripcion_extra,

                ValorFijo = entity.valor_fijo,

                Estado = entity.estado_extra,
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
        public static ExtraEntity ToEntity(ExtraDataModel model)
        {
            return new ExtraEntity
            {
                id_extra = model.Id,
                extra_guid = model.Guid,

                codigo_extra = model.Codigo,
                nombre_extra = model.Nombre,
                descripcion_extra = model.Descripcion,

                valor_fijo = model.ValorFijo,

                estado_extra = model.Estado,
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

        // 🔥 Helper para listas (recomendado)
        public static List<ExtraDataModel> ToDataModelList(IEnumerable<ExtraEntity> entities)
        {
            return entities.Select(ToDataModel).ToList();
        }
    }
}