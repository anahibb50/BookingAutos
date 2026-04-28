using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.UsuarioApp;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class UsuarioAppDataMapper
    {
        // 🔁 Entity → DataModel
        public static UsuarioAppDataModel ToDataModel(UsuarioAppEntity entity)
        {
            return new UsuarioAppDataModel
            {
                Id = entity.id_usuario,
                Guid = entity.usuario_guid,
                PasswordHash = entity.password_hash,
                PasswordSalt = entity.password_salt,

                Username = entity.username,
                Correo = entity.correo,

                IdCliente = entity.id_cliente,

                Estado = entity.estado_usuario,
                Activo = entity.activo,
                EsEliminado = entity.es_eliminado,

                FechaRegistroUtc = entity.fecha_registro_utc,
                CreadoPorUsuario = entity.creado_por_usuario,

                ModificadoPorUsuario = entity.modificado_por_usuario,
                FechaModificacionUtc = entity.fecha_modificacion_utc,

                RowVersion = entity.row_version
            };
        }

        // 🔁 DataModel → Entity
        public static UsuarioAppEntity ToEntity(UsuarioAppDataModel model)
        {
            return new UsuarioAppEntity
            {
                id_usuario = model.Id,
                usuario_guid = model.Guid,

                username = model.Username,
                correo = model.Correo,

                id_cliente = model.IdCliente,
                password_hash = model.PasswordHash,
                password_salt = model.PasswordSalt,

                estado_usuario = model.Estado,
                activo = model.Activo,
                es_eliminado = model.EsEliminado,

                fecha_registro_utc = model.FechaRegistroUtc,
                creado_por_usuario = model.CreadoPorUsuario,

                modificado_por_usuario = model.ModificadoPorUsuario,
                fecha_modificacion_utc = model.FechaModificacionUtc,

                row_version = model.RowVersion

                // ⚠️ NO tocamos password aquí
            };
        }

        // 🔥 Helper lista
        public static List<UsuarioAppDataModel> ToDataModelList(IEnumerable<UsuarioAppEntity> entities)
        {
            return entities.Select(ToDataModel).ToList();
        }
    }
}