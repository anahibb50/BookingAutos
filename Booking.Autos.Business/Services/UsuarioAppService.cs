using Booking.Autos.Business.DTOs.Usuario;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.UsuarioApp;

namespace Booking.Autos.Business.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IUsuarioAppDataService _dataService;

        public UsuarioAppService(IUsuarioAppDataService dataService)
        {
            _dataService = dataService;
        }

        // ============================================================
        // 🔥 CREAR
        // ============================================================
        public async Task<UsuarioResponse> CrearAsync(
            CrearUsuarioRequest request,
            string usuario,
            CancellationToken ct = default)
        {
            // 🔍 VALIDACIONES
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new Exception("El username es obligatorio");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new Exception("La contraseña es obligatoria");

            if (await _dataService.ExistsByUsernameAsync(request.Username, ct))
                throw new Exception("El username ya existe");

            if (await _dataService.ExistsByCorreoAsync(request.Correo, ct))
                throw new Exception("El correo ya existe");

            // 🔥 MAPPER
            var model = UsuarioBusinessMapper.ToDataModel(request, usuario);

            // 🔥 CREAR (password se manda aparte)
            var result = await _dataService.CreateAsync(model, request.Password, ct);

            return UsuarioBusinessMapper.ToResponse(result);
        }

        // ============================================================
        // ✏️ ACTUALIZAR
        // ============================================================
        public async Task<UsuarioResponse> ActualizarAsync(
            int id,
            CrearUsuarioRequest request,
            string usuario,
            CancellationToken ct = default)
        {
            var existing = await _dataService.GetByIdAsync(id, ct);

            if (existing == null)
                throw new Exception("Usuario no encontrado");

            // 🔥 actualizar campos
            existing.Username = request.Username;
            existing.Correo = request.Correo;
            existing.IdCliente = request.IdCliente;

            existing.ModificadoPorUsuario = usuario;
            existing.FechaModificacionUtc = DateTime.UtcNow;

            var updated = await _dataService.UpdateAsync(existing, ct);

            return UsuarioBusinessMapper.ToResponse(updated);
        }

        // ============================================================
        // 🔄 ACTIVAR
        // ============================================================
        public async Task<bool> ActivarAsync(int id, CancellationToken ct = default)
        {
            return await _dataService.ActivarAsync(id, ct);
        }

        // ============================================================
        // 🔄 DESACTIVAR
        // ============================================================
        public async Task<bool> DesactivarAsync(int id, CancellationToken ct = default)
        {
            return await _dataService.DesactivarAsync(id, ct);
        }

        // ============================================================
        // 🔍 OBTENER POR ID
        // ============================================================
        public async Task<UsuarioResponse?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            var model = await _dataService.GetByIdAsync(id, ct);

            return model == null ? null : UsuarioBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // 🔍 POR USERNAME
        // ============================================================
        public async Task<UsuarioResponse?> ObtenerPorUsernameAsync(string username, CancellationToken ct = default)
        {
            var model = await _dataService.GetByUsernameAsync(username, ct);

            return model == null ? null : UsuarioBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // 🔍 POR CORREO
        // ============================================================
        public async Task<UsuarioResponse?> ObtenerPorCorreoAsync(string correo, CancellationToken ct = default)
        {
            var model = await _dataService.GetByCorreoAsync(correo, ct);

            return model == null ? null : UsuarioBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // 📄 LISTAR
        // ============================================================
        public async Task<IReadOnlyList<UsuarioResponse>> ListarAsync(CancellationToken ct = default)
        {
            var list = await _dataService.GetAllAsync(ct);

            return UsuarioBusinessMapper.ToResponseList(list);
        }

        // ============================================================
        // ✅ VALIDACIONES
        // ============================================================
        public async Task<bool> ExistePorUsernameAsync(string username, CancellationToken ct = default)
        {
            return await _dataService.ExistsByUsernameAsync(username, ct);
        }

        public async Task<bool> ExistePorCorreoAsync(string correo, CancellationToken ct = default)
        {
            return await _dataService.ExistsByCorreoAsync(correo, ct);
        }
    }
}