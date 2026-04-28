using Booking.Autos.Business.DTOs.Usuario;
using Booking.Autos.DataManagement.Models.UsuarioApp;

namespace Booking.Autos.Business.Mappers
{
    public static class UsuarioBusinessMapper
    {
        // ============================================================
        // 🔥 CREATE: Request → DataModel
        // ============================================================
        public static UsuarioAppDataModel ToDataModel(CrearUsuarioRequest request, string usuario)
        {
            return new UsuarioAppDataModel
            {
                Username = request.Username,
                Correo = request.Correo,
                IdCliente = null,

                // 🔥 SISTEMA
                Guid = Guid.NewGuid(),
                Estado = "ACT",
                Activo = true,
                EsEliminado = false,
                FechaRegistroUtc = DateTime.UtcNow,
                CreadoPorUsuario = usuario
            };
        }

        // ============================================================
        // 📤 RESPONSE: DataModel → DTO
        // ============================================================
        public static UsuarioResponse ToResponse(UsuarioAppDataModel model)
        {
            return new UsuarioResponse
            {
                Id = model.Id,
                Guid = model.Guid,

                Username = model.Username,
                Correo = model.Correo,

                IdCliente = model.IdCliente,

                Estado = model.Estado,
                Activo = model.Activo
            };
        }

        // ============================================================
        // 📤 LIST: DataModel List → Response List
        // ============================================================
        public static List<UsuarioResponse> ToResponseList(IEnumerable<UsuarioAppDataModel> list)
        {
            return list.Select(ToResponse).ToList();
        }
    }
}