using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.Conductores;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class ConductorDataMapper
    {
        // 🔁 Entity → DataModel
        public static ConductorDataModel ToDataModel(ConductorEntity entity)
        {
            return new ConductorDataModel
            {
                Id = entity.id_conductor,
                Guid = entity.conductor_guid,

                Codigo = entity.codigo_conductor,

                TipoIdentificacion = entity.tipo_identificacion,
                NumeroIdentificacion = entity.numero_identificacion,

                Nombre1 = entity.con_nombre1,
                Nombre2 = entity.con_nombre2,
                Apellido1 = entity.con_apellido1,
                Apellido2 = entity.con_apellido2,

                NumeroLicencia = entity.numero_licencia,
                FechaVencimientoLicencia = entity.fecha_vencimiento_licencia,
                Edad = entity.edad_conductor,

                Telefono = entity.con_telefono,
                Correo = entity.con_correo,

                Estado = entity.estado_conductor,
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
        public static ConductorEntity ToEntity(ConductorDataModel model)
        {
            return new ConductorEntity
            {
                id_conductor = model.Id,
                conductor_guid = model.Guid,

                codigo_conductor = model.Codigo,

                tipo_identificacion = model.TipoIdentificacion,
                numero_identificacion = model.NumeroIdentificacion,

                con_nombre1 = model.Nombre1,
                con_nombre2 = model.Nombre2,
                con_apellido1 = model.Apellido1,
                con_apellido2 = model.Apellido2,

                numero_licencia = model.NumeroLicencia,
                fecha_vencimiento_licencia = model.FechaVencimientoLicencia,
                edad_conductor = model.Edad,

                con_telefono = model.Telefono,
                con_correo = model.Correo,

                estado_conductor = model.Estado,
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
    }
}