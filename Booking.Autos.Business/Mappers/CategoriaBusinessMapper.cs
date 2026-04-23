using Booking.Autos.Business.DTOs.Catalogos.Categoria;
using Booking.Autos.DataManagement.Models.Categorias;

namespace Booking.Autos.Business.Mappers
{
    public static class CategoriaBusinessMapper
    {
        public static CategoriaDataModel ToDataModel(CrearCategoriaRequest request)
        {
            return new CategoriaDataModel
            {
                Nombre = request.Nombre,
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow,
                EsEliminado = false
            };
        }

        public static CategoriaDataModel ToDataModel(ActualizarCategoriaRequest request)
        {
            return new CategoriaDataModel
            {
                Id = request.Id,
                Nombre = request.Nombre,
                FechaActualizacion = DateTime.UtcNow
            };
        }

        public static CategoriaResponse ToResponse(CategoriaDataModel model)
        {
            return new CategoriaResponse
            {
                Id = model.Id,
                Guid = model.Guid,
                Nombre = model.Nombre,
                Descripcion = model.Descripcion
            };
        }

        public static List<CategoriaResponse> ToResponseList(IEnumerable<CategoriaDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}
