using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.UsuarioApp;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class RolDataMapper
    {
        // =========================
        // 🔁 Entity → DataModel
        // =========================

        public static RolDataModel ToDataModel(RolEntity entity)
        {
            return new RolDataModel
            {
                Id = entity.id_rol,
                Guid = entity.rol_guid,

                Nombre = entity.nombre_rol,
                Descripcion = entity.descripcion_rol,

                Estado = entity.estado_rol,
                Activo = entity.activo,
                EsEliminado = entity.es_eliminado,

                FechaRegistroUtc = entity.fecha_registro_utc,
                CreadoPorUsuario = entity.creado_por_usuario,

                ModificadoPorUsuario = entity.modificado_por_usuario,
                FechaModificacionUtc = entity.fecha_modificacion_utc,

                RowVersion = entity.row_version
            };
        }

        // =========================
        // 🔁 DataModel → Entity
        // =========================

        public static RolEntity ToEntity(RolDataModel model)
        {
            return new RolEntity
            {
                id_rol = model.Id,
                rol_guid = model.Guid,

                nombre_rol = model.Nombre,
                descripcion_rol = model.Descripcion,

                estado_rol = model.Estado,
                activo = model.Activo,
                es_eliminado = model.EsEliminado,

                fecha_registro_utc = model.FechaRegistroUtc,
                creado_por_usuario = model.CreadoPorUsuario,

                modificado_por_usuario = model.ModificadoPorUsuario,
                fecha_modificacion_utc = model.FechaModificacionUtc,

                row_version = model.RowVersion
            };
        }

        // =========================
        // 🔥 Helper lista
        // =========================

        public static List<RolDataModel> ToDataModelList(IEnumerable<RolEntity> entities)
        {
            return entities.Select(ToDataModel).ToList();
        }
    }
}