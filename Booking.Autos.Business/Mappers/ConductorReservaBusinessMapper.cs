using Booking.Autos.Business.DTOs.ConductorReserva;
using Booking.Autos.DataManagement.Models.Conductores;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.Business.Mappers
{
    public static class ConductorReservaBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static ConductorReservaDataModel ToDataModel(AssignConductorRequest request)
        {
            return new ConductorReservaDataModel
            {
                IdReserva = request.IdReserva,
                IdConductor = request.IdConductor,

                Rol = request.Rol,
                EsPrincipal = request.EsPrincipal,
                Observaciones = request.Observaciones,

                // 🔥 estado inicial
                Estado = "ACT",
                EsEliminado = false,

                // 🔥 auditoría mínima
                FechaRegistroUtc = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE (RECOMENDADO)
        // =========================
        public static ConductorReservaResponse ToResponse(ConductorReservaDataModel model)
        {
            return new ConductorReservaResponse
            {
                Id = model.Id,
                Guid = model.Guid,

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
        public static List<ConductorReservaResponse> ToResponseList(IEnumerable<ConductorReservaDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}