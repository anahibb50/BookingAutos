using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.Clientes;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class ClienteDataMapper
    {
        // 🔁 Entity → DataModel
        public static ClienteDataModel ToDataModel(ClienteEntity entity)
        {
            return new ClienteDataModel
            {
                Id = entity.id_cliente,
                Guid = entity.cliente_guid,

                Nombre = entity.cli_nombre,
                Apellido = entity.cli_apellido,
                RazonSocial = entity.razon_social,

                TipoIdentificacion = entity.tipo_identificacion,
                Identificacion = entity.cli_ruc_ced,

                IdCiudad = entity.id_ciudad,
                Direccion = entity.cli_direccion,
                Genero = entity.cli_genero,

                Telefono = entity.cli_telefono,
                Email = entity.cli_email,

                Estado = entity.cli_estado,

                CreadoPorUsuario = entity.creado_por_usuario,
                FechaRegistroUtc = entity.fecha_registro_utc,

                ModificadoPorUsuario = entity.modificado_por_usuario,
                FechaModificacionUtc = entity.fecha_modificacion_utc,
                ModificacionIp = entity.modificacion_ip,
                ServicioOrigen = entity.servicio_origen,

                EsEliminado = entity.es_eliminado,
                FechaEliminacion = entity.fecha_eliminacion
            };
        }

        // 🔁 DataModel → Entity
        public static ClienteEntity ToEntity(ClienteDataModel model)
        {
            return new ClienteEntity
            {
                id_cliente = model.Id,
                cliente_guid = model.Guid,

                cli_nombre = model.Nombre,
                cli_apellido = model.Apellido,
                razon_social = model.RazonSocial,

                tipo_identificacion = model.TipoIdentificacion,
                cli_ruc_ced = model.Identificacion,

                id_ciudad = model.IdCiudad,
                cli_direccion = model.Direccion,
                cli_genero = model.Genero,

                cli_telefono = model.Telefono,
                cli_email = model.Email,

                cli_estado = model.Estado,

                creado_por_usuario = model.CreadoPorUsuario,
                fecha_registro_utc = model.FechaRegistroUtc,

                modificado_por_usuario = model.ModificadoPorUsuario,
                fecha_modificacion_utc = model.FechaModificacionUtc,
                modificacion_ip = model.ModificacionIp,
                servicio_origen = model.ServicioOrigen,

                es_eliminado = model.EsEliminado,
                fecha_eliminacion = model.FechaEliminacion
            };
        }
    }
}