using Booking.Autos.Business.DTOs.Conductor;
using Booking.Autos.DataManagement.Models.Conductores;

namespace Booking.Autos.Business.Mappers
{
    public static class ConductorBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static ConductorDataModel ToDataModel(CrearConductorRequest request)
        {
            return new ConductorDataModel
            {
                // 🔑 básicos

                TipoIdentificacion = request.TipoIdentificacion,
                NumeroIdentificacion = request.NumeroIdentificacion,

                Nombre1 = request.Nombre1,
                Nombre2 = request.Nombre2,
                Apellido1 = request.Apellido1,
                Apellido2 = request.Apellido2,

                NumeroLicencia = request.NumeroLicencia,
                FechaVencimientoLicencia = request.FechaVencimientoLicencia,

                Edad = request.Edad,

                Telefono = request.Telefono,
                Correo = request.Correo,

                // 🔥 auditoría mínima
                FechaRegistroUtc = DateTime.UtcNow
            };
        }

        // =========================
        // ACTUALIZAR → DATAMODEL
        // =========================
        public static ConductorDataModel ToDataModel(ActualizarConductorRequest request)
        {
            return new ConductorDataModel
            {
                Id = request.Id,

               

                TipoIdentificacion = request.TipoIdentificacion,
                NumeroIdentificacion = request.NumeroIdentificacion,

                Nombre1 = request.Nombre1,
                Nombre2 = request.Nombre2,
                Apellido1 = request.Apellido1,
                Apellido2 = request.Apellido2,

                NumeroLicencia = request.NumeroLicencia,
                FechaVencimientoLicencia = request.FechaVencimientoLicencia,

                Edad = request.Edad,

                Telefono = request.Telefono,
                Correo = request.Correo,

                Estado = request.Estado,

                // 🔥 auditoría
                FechaModificacionUtc = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static ConductorResponse ToResponse(ConductorDataModel model)
        {
            return new ConductorResponse
            {
                Id = model.Id,
                Guid = model.Guid,

                Codigo = model.Codigo,

                TipoIdentificacion = model.TipoIdentificacion,
                NumeroIdentificacion = model.NumeroIdentificacion,

                Nombre1 = model.Nombre1,
                Nombre2 = model.Nombre2,
                Apellido1 = model.Apellido1,
                Apellido2 = model.Apellido2,

                NumeroLicencia = model.NumeroLicencia,
                FechaVencimientoLicencia = model.FechaVencimientoLicencia,

                Edad = model.Edad,

                Telefono = model.Telefono,
                Correo = model.Correo,

                Estado = model.Estado
            };
        }

        // =========================
        // LISTA
        // =========================
        public static List<ConductorResponse> ToResponseList(IEnumerable<ConductorDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}