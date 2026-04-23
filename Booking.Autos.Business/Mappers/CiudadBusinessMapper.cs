using Booking.Autos.Business.DTOs.Catalogos.Ciudad;
using Booking.Autos.Business.DTOs.Ciudad;
using Booking.Autos.DataManagement.Models.Ciudades;

namespace Booking.Autos.Business.Mappers
{
    public static class CiudadBusinessMapper
    {
        public static CiudadDataModel ToDataModel(CrearCiudadRequest request)
        {
            return new CiudadDataModel
            {
                Nombre = request.Nombre,
                CodigoPostal = request.CodigoPostal,
                IdPais = request.IdPais,
                Estado = "ACT",
                OrigenRegistro = "API",
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow,
                EsEliminado = false
            };
        }

        public static CiudadDataModel ToDataModel(ActualizarCiudadRequest request)
        {
            return new CiudadDataModel
            {
                Id = request.Id,
                Nombre = request.Nombre,
                CodigoPostal = request.CodigoPostal,
                IdPais = request.IdPais,
                FechaActualizacion = DateTime.UtcNow
            };
        }

        public static CiudadResponse ToResponse(CiudadDataModel model)
        {
            return new CiudadResponse
            {
                Id = model.Id,
                Guid = model.Guid,
                Nombre = model.Nombre,
                CodigoPostal = model.CodigoPostal,
                IdPais = model.IdPais
            };
        }

        public static List<CiudadResponse> ToResponseList(IEnumerable<CiudadDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}
