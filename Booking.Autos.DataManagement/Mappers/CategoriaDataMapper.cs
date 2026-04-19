using Booking.Autos.DataAccess.Entities;

using Booking.Autos.DataManagement.Models.Categorias;

namespace Booking.Autos.DataManagement.Mappers
{
    public static class CategoriaDataMapper
    {
        // 🔁 Entity → DataModel
        public static CategoriaDataModel ToDataModel(CategoriaEntity entity)
        {
            return new CategoriaDataModel
            {
                Id = entity.id_categoria,
                Guid = entity.categoria_guid,
                Nombre = entity.nombre_categoria,

                FechaCreacion = entity.fecha_creacion,
                FechaActualizacion = entity.fecha_actualizacion,
                FechaEliminacion = entity.fecha_eliminacion,
                EsEliminado = entity.es_eliminado
            };
        }

        // 🔁 DataModel → Entity
        public static CategoriaEntity ToEntity(CategoriaDataModel model)
        {
            return new CategoriaEntity
            {
                id_categoria = model.Id,
                categoria_guid = model.Guid,
                nombre_categoria = model.Nombre,

                fecha_creacion = model.FechaCreacion,
                fecha_actualizacion = model.FechaActualizacion,
                fecha_eliminacion = model.FechaEliminacion,
                es_eliminado = model.EsEliminado
            };
        }
    }
}