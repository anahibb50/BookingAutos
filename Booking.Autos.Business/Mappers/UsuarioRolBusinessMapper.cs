using Booking.Autos.Business.DTOs.UsuarioRol;
using Booking.Autos.DataManagement.Models.UsuarioApp;

namespace Booking.Autos.Business.Mappers
{
    public static class UsuarioRolBusinessMapper
    {
        // ============================================================
        // 🔥 CREATE: Request → DataModel
        // ============================================================
        public static UsuarioRolDataModel ToDataModel(CrearUsuarioRolRequest request, string usuario)
        {
            return new UsuarioRolDataModel
            {
                IdUsuario = request.IdUsuario,
                IdRol = request.IdRol,
                Activo = request.Activo,

                // 🔥 SISTEMA
                Estado = "ACT",
                EsEliminado = false,
                FechaRegistroUtc = DateTime.UtcNow,
                CreadoPorUsuario = usuario
            };
        }

        // ============================================================
        // 🔥 UPDATE: Request → DataModel
        // ============================================================
        public static UsuarioRolDataModel ToDataModel(
            ActualizarUsuarioRolRequest request,
            UsuarioRolDataModel existing,
            string usuario)
        {
            // ✏️ actualizar campos editables
            existing.IdRol = request.IdRol;
            existing.Activo = request.Activo;

            // 🔥 auditoría
            existing.ModificadoPorUsuario = usuario;
            existing.FechaModificacionUtc = DateTime.UtcNow;

            return existing;
        }

        // ============================================================
        // 📤 RESPONSE: DataModel → DTO
        // ============================================================
        public static UsuarioRolResponse ToResponse(UsuarioRolDataModel model)
        {
            return new UsuarioRolResponse
            {
                IdUsuarioRol = model.Id,

                IdUsuario = model.IdUsuario,
                IdRol = model.IdRol,

                Estado = model.Estado,
                Activo = model.Activo,

                FechaRegistroUtc = model.FechaRegistroUtc
            };
        }

        // ============================================================
        // 📤 LIST: DataModel List → Response List
        // ============================================================
        public static List<UsuarioRolResponse> ToResponseList(IEnumerable<UsuarioRolDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}