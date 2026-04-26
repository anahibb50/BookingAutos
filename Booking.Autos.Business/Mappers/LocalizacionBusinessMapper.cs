using Booking.Autos.Business.DTOs.Localizacion;
using Booking.Autos.DataManagement.Models.Localizaciones;

namespace Booking.Autos.Business.Mappers
{
    public static class LocalizacionBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static LocalizacionDataModel ToDataModel(CrearLocalizacionRequest request)
        {
            return new LocalizacionDataModel
            {

                Nombre = request.Nombre,
                IdCiudad = request.IdCiudad,

                Direccion = request.Direccion,
                Telefono = request.Telefono,
                Correo = request.Correo,

                HorarioAtencion = request.HorarioAtencion,
                ZonaHoraria = request.ZonaHoraria,

                // 🔥 estado inicial
                Estado = "ACT",
                EsEliminado = false,

                // 🔥 auditoría mínima
                FechaRegistroUtc = DateTime.UtcNow
            };
        }

        // =========================
        // ACTUALIZAR → DATAMODEL
        // =========================
        public static LocalizacionDataModel ToDataModel(ActualizarLocalizacionRequest request)
        {
            return new LocalizacionDataModel
            {
                Id = request.Id,


                Nombre = request.Nombre,
                IdCiudad = request.IdCiudad,

                Direccion = request.Direccion,
                Telefono = request.Telefono,
                Correo = request.Correo,

                HorarioAtencion = request.HorarioAtencion,
                ZonaHoraria = request.ZonaHoraria,

                // 🔥 auditoría
                FechaModificacionUtc = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static LocalizacionResponse ToResponse(LocalizacionDataModel model)
        {
            return new LocalizacionResponse
            {
                Id = model.Id,
                Guid = model.Guid,
                Codigo = model.Codigo,

                Nombre = model.Nombre,
                IdCiudad = model.IdCiudad,

                Direccion = model.Direccion,
                Telefono = model.Telefono,
                Correo = model.Correo,

                HorarioAtencion = model.HorarioAtencion,
                ZonaHoraria = model.ZonaHoraria,

                Estado = model.Estado
            };
        }

        // =========================
        // LISTA
        // =========================
        public static List<LocalizacionResponse> ToResponseList(IEnumerable<LocalizacionDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}