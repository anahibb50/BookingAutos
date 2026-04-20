using Booking.Autos.Business.DTOs.Reserva;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.Business.Mappers
{
    public static class ReservaBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static ReservaDataModel ToDataModel(CrearReservaRequest request)
        {
            return new ReservaDataModel
            {

                IdCliente = request.IdCliente,
                IdVehiculo = request.IdVehiculo,

                IdLocalizacionRecogida = request.IdLocalizacionRecogida,
                IdLocalizacionEntrega = request.IdLocalizacionEntrega,

                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,

                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,

                Descripcion = request.Descripcion,
                OrigenCanal = request.OrigenCanal,

                // 🔥 valores iniciales (se calculan bien en service)
             
                // 🔥 estado inicial
                Estado = "PEN",

                // 🔥 auditoría mínima
                FechaReservaUtc = DateTime.UtcNow,
                EsEliminado = false
            };
        }

        // =========================
        // ACTUALIZAR → DATAMODEL
        // =========================
        public static ReservaDataModel ToDataModel(ActualizarReservaRequest request)
        {
            return new ReservaDataModel
            {
                Id = request.Id,

           

                IdLocalizacionRecogida = request.IdLocalizacionRecogida,
                IdLocalizacionEntrega = request.IdLocalizacionEntrega,

                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,

                HoraInicio = request.HoraInicio,
                HoraFin = request.HoraFin,

                Descripcion = request.Descripcion,


            
                

                // 🔥 auditoría
                FechaModificacionUtc = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static ReservaResponse ToResponse(ReservaDataModel model)
        {
            return new ReservaResponse
            {
                Id = model.Id,
                Guid = model.Guid,
                Codigo = model.Codigo,

                IdCliente = model.IdCliente,
                IdVehiculo = model.IdVehiculo,

                IdLocalizacionRecogida = model.IdLocalizacionRecogida,
                IdLocalizacionEntrega = model.IdLocalizacionEntrega,

                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,

                HoraInicio = model.HoraInicio,
                HoraFin = model.HoraFin,

                CantidadDias = model.CantidadDias,

                Subtotal = model.Subtotal,
                Iva = model.Iva,
                Total = model.Total,

                Descripcion = model.Descripcion,
                OrigenCanal = model.OrigenCanal,

                Estado = model.Estado,

                FechaConfirmacionUtc = model.FechaConfirmacionUtc,
                FechaCancelacionUtc = model.FechaCancelacionUtc,
                MotivoCancelacion = model.MotivoCancelacion
            };
        }

        // =========================
        // LISTA
        // =========================
        public static List<ReservaResponse> ToResponseList(IEnumerable<ReservaDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}