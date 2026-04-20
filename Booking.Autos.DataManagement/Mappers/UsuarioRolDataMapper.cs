using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.UsuarioApp;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class UsuarioRolDataMapper
    {
        // =========================
        // 🔁 Entity → DataModel
        // =========================

        public static UsuarioRolDataModel ToDataModel(UsuarioRolEntity entity)
        {
            return new UsuarioRolDataModel
            {
                Id = entity.id_usuario_rol,

                IdUsuario = entity.id_usuario,
                IdRol = entity.id_rol,

                Estado = entity.estado_usuario_rol,
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

        public static UsuarioRolEntity ToEntity(UsuarioRolDataModel model)
        {
            return new UsuarioRolEntity
            {
                id_usuario_rol = model.Id,

                id_usuario = model.IdUsuario,
                id_rol = model.IdRol,

                estado_usuario_rol = model.Estado,
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

        public static List<UsuarioRolDataModel> ToDataModelList(IEnumerable<UsuarioRolEntity> entities)
        {
            return entities.Select(ToDataModel).ToList();
        }
    }
}