using Booking.Autos.Business.DTOs.Rol;
using Booking.Autos.DataManagement.Models.UsuarioApp;

namespace Booking.Autos.Business.Mappers
{
    public static class RolBusinessMapper
    {
        // ============================================================
        // 🔥 CREATE: Request → DataModel
        // ============================================================
        public static RolDataModel ToDataModel(CrearRolRequest request, string usuario)
        {
            return new RolDataModel
            {
                Nombre = request.NombreRol,
                Descripcion = request.DescripcionRol,
                Activo = request.Activo,

                // 🔥 SISTEMA
                Guid = Guid.NewGuid(),
                Estado = "ACT",
                EsEliminado = false,
                FechaRegistroUtc = DateTime.UtcNow,
                CreadoPorUsuario = usuario
            };
        }

        // ============================================================
        // 🔥 UPDATE: Request → DataModel
        // ============================================================
        public static RolDataModel ToDataModel(ActualizarRolRequest request, RolDataModel existing, string usuario)
        {
            // ✏️ actualiza sobre el existente
            existing.Nombre = request.NombreRol;
            existing.Descripcion = request.DescripcionRol;
            existing.Activo = request.Activo;

            // 🔥 AUDITORÍA
            existing.ModificadoPorUsuario = usuario;
            existing.FechaModificacionUtc = DateTime.UtcNow;

            return existing;
        }

        // ============================================================
        // 📤 RESPONSE: DataModel → DTO
        // ============================================================
        public static RolResponse ToResponse(RolDataModel model)
        {
            return new RolResponse
            {
                IdRol = model.Id,
                RolGuid = model.Guid,

                NombreRol = model.Nombre,
                DescripcionRol = model.Descripcion,

                Estado = model.Estado,
                Activo = model.Activo,

                FechaRegistroUtc = model.FechaRegistroUtc
            };
        }

        // ============================================================
        // 📤 LIST: DataModel List → Response List
        // ============================================================
        public static List<RolResponse> ToResponseList(IEnumerable<RolDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}