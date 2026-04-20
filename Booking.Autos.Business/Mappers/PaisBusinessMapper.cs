using Booking.Autos.Business.DTOs.Catalogos.Pais;
using Booking.Autos.Business.DTOs.Pais;
using Booking.Autos.DataManagement.Models.Paises;

namespace Booking.Autos.Business.Mappers
{
    public static class PaisBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static PaisDataModel ToDataModel(CrearPaisRequest request)
        {
            return new PaisDataModel
            {
                Nombre = request.Nombre,
                CodigoIso = request.CodigoIso,

                // 🔥 campos del modelo
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow,
                EsEliminado = false
            };
        }

        // =========================
        // ACTUALIZAR → DATAMODEL
        // =========================
        public static PaisDataModel ToDataModel(ActualizarPaisRequest request)
        {
            return new PaisDataModel
            {
                Id = request.Id,
                Nombre = request.Nombre,
                CodigoIso = request.CodigoIso,

                // 🔥 auditoría
                FechaActualizacion = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static PaisResponse ToResponse(PaisDataModel model)
        {
            return new PaisResponse
            {
                Id = model.Id,
                Guid = model.Guid,
                Nombre = model.Nombre,
                CodigoIso = model.CodigoIso
            };
        }

        // =========================
        // LISTA
        // =========================
        public static List<PaisResponse> ToResponseList(IEnumerable<PaisDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}