using Booking.Autos.Business.DTOs.ConductorReserva;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.Business.Mappers
{
    public static class ConductorReservaBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static ConductorReservaDataModel ToDataModel(CrearConductorReservaDetalleRequest request)
        {
            return new ConductorReservaDataModel
            {
                // IdReserva se asigna en el service

                IdConductor = request.IdConductor,

                Rol = request.Rol,
                EsPrincipal = request.EsPrincipal,
                Observaciones = request.Observaciones,

                Estado = "PEN",

                FechaRegistroUtc = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static ConductorReservaDetalleResponse ToResponse(ConductorReservaDataModel model)
        {
            return new ConductorReservaDetalleResponse
            {


                IdReserva = model.IdReserva,
                IdConductor = model.IdConductor,

                Rol = model.Rol,
                EsPrincipal = model.EsPrincipal,
                Observaciones = model.Observaciones,

                Estado = model.Estado
            };
        }

        // =========================
        // LISTA
        // =========================
        public static List<ConductorReservaDetalleResponse> ToResponseList(IEnumerable<ConductorReservaDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}
