using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataManagement.Models.Marcas;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class MarcaDataMapper
    {
        // 🔁 Entity → DataModel
        public static MarcaDataModel ToDataModel(MarcaEntity entity)
        {
            return new MarcaDataModel
            {
                Id = entity.id_marca,
                Guid = entity.marca_guid,

                Nombre = entity.nombre_marca,

                FechaCreacion = entity.fecha_creacion,
                FechaActualizacion = entity.fecha_actualizacion,
                FechaEliminacion = entity.fecha_eliminacion,

                EsEliminado = entity.es_eliminado
            };
        }

        // 🔁 DataModel → Entity
        public static MarcaEntity ToEntity(MarcaDataModel model)
        {
            return new MarcaEntity
            {
                id_marca = model.Id,
                marca_guid = model.Guid,

                nombre_marca = model.Nombre,

                fecha_creacion = model.FechaCreacion,
                fecha_actualizacion = model.FechaActualizacion,
                fecha_eliminacion = model.FechaEliminacion,

                es_eliminado = model.EsEliminado
            };
        }

        // 🔥 Helper lista
        public static List<MarcaDataModel> ToDataModelList(IEnumerable<MarcaEntity> entities)
        {
            return entities.Select(ToDataModel).ToList();
        }
    }
}