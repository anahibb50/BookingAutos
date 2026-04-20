using Booking.Autos.Business.DTOs.Catalogos.Marca;
using Booking.Autos.Business.DTOs.Marca;
using Booking.Autos.DataManagement.Models.Marcas;

namespace Booking.Autos.Business.Mappers
{
    public static class MarcaBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static MarcaDataModel ToDataModel(CrearMarcaRequest request)
        {
            return new MarcaDataModel
            {
                Nombre = request.Nombre,

                // 🔥 campos del modelo
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow,
                EsEliminado = false
            };
        }

        // =========================
        // ACTUALIZAR → DATAMODEL
        // =========================
        public static MarcaDataModel ToDataModel(ActualizarMarcaRequest request)
        {
            return new MarcaDataModel
            {
                Id = request.Id,
                Nombre = request.Nombre,

                // 🔥 auditoría
                FechaActualizacion = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static MarcaResponse ToResponse(MarcaDataModel model)
        {
            return new MarcaResponse
            {
                Id = model.Id,
                Guid = model.Guid,
                Nombre = model.Nombre
            };
        }

        // =========================
        // LISTA
        // =========================
        public static List<MarcaResponse> ToResponseList(IEnumerable<MarcaDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}