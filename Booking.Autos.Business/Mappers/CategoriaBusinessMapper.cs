using Booking.Autos.Business.DTOs.Catalogos.Categoria;

using Booking.Autos.DataManagement.Models.Categorias;

namespace Booking.Autos.Business.Mappers
{
    public static class CategoriaBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static CategoriaDataModel ToDataModel(CrearCategoriaRequest request)
        {
            return new CategoriaDataModel
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,

                // 🔥 CAMPOS REALES DEL MODELO
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow,
                EsEliminado = false
            };
        }

        // =========================
        // ACTUALIZAR → DATAMODEL
        // =========================
        public static CategoriaDataModel ToDataModel(ActualizarCategoriaRequest request)
        {
            return new CategoriaDataModel
            {
                Id = request.Id,
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,

                // 🔥 SOLO LO QUE REALMENTE EXISTE
                FechaActualizacion = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
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

        // =========================
        // LISTA
        // =========================
        public static List<CategoriaResponse> ToResponseList(IEnumerable<CategoriaDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}