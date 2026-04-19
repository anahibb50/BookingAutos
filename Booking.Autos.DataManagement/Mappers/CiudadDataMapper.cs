using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.Ciudades;


namespace Booking.Autos.DataManagement.Mappers
{
    public static class CiudadDataMapper
    {
        // 🔁 Entity → DataModel
        public static CiudadDataModel ToDataModel(CiudadEntity entity)
        {
            return new CiudadDataModel
            {
                Id = entity.id_ciudad,
                Guid = entity.ciudad_guid,
                Nombre = entity.nombre_ciudad,
                CodigoPostal = entity.codigo_postal,
                IdPais = entity.id_pais,
                Estado = entity.estado_ciudad,
                OrigenRegistro = entity.origen_registro,

                FechaCreacion = entity.fecha_creacion,
                FechaActualizacion = entity.fecha_actualizacion,
                FechaInhabilitacion = entity.fecha_inhabilitacion,
                MotivoInhabilitacion = entity.motivo_inhabilitacion,
                EsEliminado = entity.es_eliminado,
                FechaEliminacion = entity.fecha_eliminacion
            };
        }

        // 🔁 DataModel → Entity
        public static CiudadEntity ToEntity(CiudadDataModel model)
        {
            return new CiudadEntity
            {
                id_ciudad = model.Id,
                ciudad_guid = model.Guid,
                nombre_ciudad = model.Nombre,
                codigo_postal = model.CodigoPostal,
                id_pais = model.IdPais,
                estado_ciudad = model.Estado,
                origen_registro = model.OrigenRegistro,

                fecha_creacion = model.FechaCreacion,
                fecha_actualizacion = model.FechaActualizacion,
                fecha_inhabilitacion = model.FechaInhabilitacion,
                motivo_inhabilitacion = model.MotivoInhabilitacion,
                es_eliminado = model.EsEliminado,
                fecha_eliminacion = model.FechaEliminacion
            };
        }
    }
}