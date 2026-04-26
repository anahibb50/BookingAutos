using Booking.Autos.Business.DTOs.Extra;
using Booking.Autos.DataManagement.Models.Extras;

namespace Booking.Autos.Business.Mappers
{
    public static class ExtraBusinessMapper
    {
        // =========================
        // CREAR → DATAMODEL
        // =========================
        public static ExtraDataModel ToDataModel(CrearExtraRequest request)
        {
            return new ExtraDataModel
            {
                // ❌ NO mandamos Codigo desde request (lo genera el backend)

                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                ValorFijo = request.ValorFijo,

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
        public static ExtraDataModel ToDataModel(ActualizarExtraRequest request)
        {
            return new ExtraDataModel
            {
                Id = request.Id,

                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                ValorFijo = request.ValorFijo,

                // 🔥 auditoría
                FechaModificacionUtc = DateTime.UtcNow
            };
        }

        // =========================
        // DATAMODEL → RESPONSE
        // =========================
        public static ExtraResponse ToResponse(ExtraDataModel model)
        {
            return new ExtraResponse
            {
                Id = model.Id,
                Guid = model.Guid,
                Codigo = model.Codigo,

                Nombre = model.Nombre,
                Descripcion = model.Descripcion,

                ValorFijo = model.ValorFijo,

                Estado = model.Estado
            };
        }

        // =========================
        // LISTA
        // =========================
        public static List<ExtraResponse> ToResponseList(IEnumerable<ExtraDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}
