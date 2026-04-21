using Booking.Autos.Business.DTOs.UsuarioRol;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Interfaces;

namespace Booking.Autos.Business.Services
{
    public class UsuarioRolService : IUsuarioRolService
    {
        private readonly IUsuarioRolDataService _dataService;

        public UsuarioRolService(IUsuarioRolDataService dataService)
        {
            _dataService = dataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<UsuarioRolResponse> CrearAsync(
            CrearUsuarioRolRequest request,
            CancellationToken ct = default)
        {
            if (request.IdUsuario <= 0)
                throw new Exception("IdUsuario inválido");

            if (request.IdRol <= 0)
                throw new Exception("IdRol inválido");

            // 🔥 evitar duplicados
            if (await _dataService.ExistsAsync(request.IdUsuario, request.IdRol, ct))
                throw new Exception("El usuario ya tiene este rol");

            var model = UsuarioRolBusinessMapper.ToDataModel(request, "SYSTEM");

            await _dataService.AssignAsync(model, ct);

            // 🔥 recuperar (opcional, pero limpio)
            var list = await _dataService.GetByUsuarioAsync(request.IdUsuario, ct);

            var creado = list.First(x => x.IdRol == request.IdRol);

            return UsuarioRolBusinessMapper.ToResponse(creado);
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<UsuarioRolResponse> ActualizarAsync(
            ActualizarUsuarioRolRequest request,
            CancellationToken ct = default)
        {
            if (request.IdUsuarioRol <= 0)
                throw new Exception("IdUsuarioRol inválido");

            var existing = await _dataService.GetByIdAsync(request.IdUsuarioRol);

            if (existing == null)
                throw new Exception("UsuarioRol no encontrado");

            var updated = UsuarioRolBusinessMapper.ToDataModel(
                request,
                existing,
                "SYSTEM"
            );

            await _dataService.UpdateAsync(updated, ct);

            return UsuarioRolBusinessMapper.ToResponse(updated);
        }

        // =========================
        // ELIMINAR
        // =========================
        public async Task EliminarAsync(
            int id,
            CancellationToken ct = default)
        {
            if (id <= 0)
                throw new Exception("Id inválido");

            var exists = await _dataService.ExistsByIdAsync(id);

            if (!exists)
                throw new Exception("UsuarioRol no existe");

            await _dataService.DeleteAsync(id);
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<UsuarioRolResponse?> ObtenerPorIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var model = await _dataService.GetByIdAsync(id);

            return model == null
                ? null
                : UsuarioRolBusinessMapper.ToResponse(model);
        }

        public async Task<IReadOnlyList<UsuarioRolResponse>> ObtenerPorUsuarioAsync(
            int idUsuario,
            CancellationToken ct = default)
        {
            var list = await _dataService.GetByUsuarioAsync(idUsuario, ct);

            return UsuarioRolBusinessMapper.ToResponseList(list);
        }

        public async Task<IReadOnlyList<UsuarioRolResponse>> ObtenerPorRolAsync(
            int idRol,
            CancellationToken ct = default)
        {
            var list = await _dataService.GetByRolAsync(idRol, ct);

            return UsuarioRolBusinessMapper.ToResponseList(list);
        }

        public async Task<IReadOnlyList<UsuarioRolResponse>> FiltrarAsync(
            UsuarioRolFiltroRequest request,
            CancellationToken ct = default)
        {
            // 🔥 simple (puedes mejorar luego con query avanzada)
            var list = await _dataService.GetByUsuarioAsync(request.IdUsuario ?? 0, ct);

            var query = list.AsQueryable();

            if (request.IdRol.HasValue)
                query = query.Where(x => x.IdRol == request.IdRol.Value);

            if (request.Activo.HasValue)
                query = query.Where(x => x.Activo == request.Activo.Value);

            if (!string.IsNullOrWhiteSpace(request.Estado))
                query = query.Where(x => x.Estado == request.Estado);

            return UsuarioRolBusinessMapper.ToResponseList(query);
        }

        // =========================
        // VALIDACIÓN
        // =========================

        public async Task<bool> ExisteAsync(
            int idUsuario,
            int idRol,
            CancellationToken ct = default)
        {
            return await _dataService.ExistsAsync(idUsuario, idRol, ct);
        }
    }
}