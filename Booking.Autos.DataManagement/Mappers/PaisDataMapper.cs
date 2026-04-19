using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.Paises;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class PaisDataMapper
    {
        // 🔁 Entity → DataModel
        public static PaisDataModel ToDataModel(PaisEntity entity)
        {
            return new PaisDataModel
            {
                Id = entity.id_pais,
                Guid = entity.pais_guid,

                Nombre = entity.nombre_pais,
                CodigoIso = entity.codigo_iso,

                FechaCreacion = entity.fecha_creacion,
                FechaActualizacion = entity.fecha_actualizacion,
                FechaEliminacion = entity.fecha_eliminacion,

                EsEliminado = entity.es_eliminado
            };
        }

        // 🔁 DataModel → Entity
        public static PaisEntity ToEntity(PaisDataModel model)
        {
            return new PaisEntity
            {
                id_pais = model.Id,
                pais_guid = model.Guid,

                nombre_pais = model.Nombre,
                codigo_iso = model.CodigoIso,

                fecha_creacion = model.FechaCreacion,
                fecha_actualizacion = model.FechaActualizacion,
                fecha_eliminacion = model.FechaEliminacion,

                es_eliminado = model.EsEliminado
            };
        }

        // 🔥 Helper lista
        public static List<PaisDataModel> ToDataModelList(IEnumerable<PaisEntity> entities)
        {
            return entities.Select(ToDataModel).ToList();
        }
    }
}