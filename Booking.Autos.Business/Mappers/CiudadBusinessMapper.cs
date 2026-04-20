using Booking.Autos.Business.DTOs.Catalogos.Ciudad;
using Booking.Autos.Business.DTOs.Ciudad;
using Booking.Autos.DataManagement.Models.Ciudades;

namespace Booking.Autos.Business.Mappers
{
    public static class CiudadBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static CiudadDataModel ToDataModel(CrearCiudadRequest request)
        {
            return new CiudadDataModel
            {
                Nombre = request.Nombre,
                IdPais = request.IdPais,

                // 🔥 campos del modelo (no del DTO)
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow,
                EsEliminado = false
            };
        }

        // =========================
        // ACTUALIZAR → DATAMODEL
        // =========================
        public static CiudadDataModel ToDataModel(ActualizarCiudadRequest request)
        {
            return new CiudadDataModel
            {
                Id = request.Id,
                Nombre = request.Nombre,
                IdPais = request.IdPais,

                // 🔥 auditoría
                FechaActualizacion = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static CiudadResponse ToResponse(CiudadDataModel model)
        {
            return new CiudadResponse
            {
                Id = model.Id,
                Guid = model.Guid,
                Nombre = model.Nombre,
                IdPais = model.IdPais
            };
        }

        // =========================
        // LISTA
        // =========================
        public static List<CiudadResponse> ToResponseList(IEnumerable<CiudadDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}