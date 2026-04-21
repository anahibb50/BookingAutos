using Booking.Autos.Business.DTOs.Rol;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.UsuarioApp;

namespace Booking.Autos.Business.Services
{
    public class RolService : IRolService
    {
        private readonly IRolDataService _dataService;

        public RolService(IRolDataService dataService)
        {
            _dataService = dataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<RolResponse> CrearAsync(
            CrearRolRequest request,
            CancellationToken ct = default)
        {
            // 🔥 VALIDACIÓN
            if (string.IsNullOrWhiteSpace(request.NombreRol))
                throw new Exception("El nombre del rol es obligatorio.");

            // 🔥 REGLA: nombre único
            if (await _dataService.ExistsByNombreAsync(request.NombreRol, ct))
                throw new Exception("Ya existe un rol con ese nombre.");

            // 🔥 MAPPER
            var model = RolBusinessMapper.ToDataModel(request, "SYSTEM");

            // 💾 GUARDAR
            var created = await _dataService.CreateAsync(model, ct);

            return RolBusinessMapper.ToResponse(created);
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<RolResponse> ActualizarAsync(
            ActualizarRolRequest request,
            CancellationToken ct = default)
        {
            if (request.IdRol <= 0)
                throw new Exception("Id inválido.");

            var existing = await _dataService.GetByIdAsync(request.IdRol, ct);

            if (existing == null)
                throw new Exception("Rol no encontrado.");

            // 🔥 VALIDAR NOMBRE (si cambia)
            if (!string.Equals(existing.Nombre, request.NombreRol, StringComparison.OrdinalIgnoreCase))
            {
                if (await _dataService.ExistsByNombreAsync(request.NombreRol, ct))
                    throw new Exception("Ya existe un rol con ese nombre.");
            }

            // 🔥 MAPPER (update sobre existing)
            var updatedModel = RolBusinessMapper.ToDataModel(request, existing, "SYSTEM");

            var updated = await _dataService.UpdateAsync(updatedModel, ct);

            return RolBusinessMapper.ToResponse(updated);
        }

        // =========================
        // ELIMINAR (LÓGICO)
        // =========================
        public async Task EliminarAsync(
            int id,
            CancellationToken ct = default)
        {
            if (id <= 0)
                throw new Exception("Id inválido.");

            var existing = await _dataService.GetByIdAsync(id, ct);

            if (existing == null)
                throw new Exception("Rol no encontrado.");

            // 🔥 no eliminar si ya está eliminado
            if (existing.EsEliminado)
                throw new Exception("El rol ya está eliminado.");

            await _dataService.DeleteAsync(id, ct);
        }

        // =========================
        // OBTENER POR ID
        // =========================
        public async Task<RolResponse?> ObtenerPorIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var model = await _dataService.GetByIdAsync(id, ct);

            return model == null
                ? null
                : RolBusinessMapper.ToResponse(model);
        }

        // =========================
        // LISTAR
        // =========================
        public async Task<IReadOnlyList<RolResponse>> ListarAsync(
            CancellationToken ct = default)
        {
            var list = await _dataService.GetAllAsync(ct);

            return RolBusinessMapper.ToResponseList(list);
        }

        // =========================
        // FILTRAR (EN MEMORIA)
        // =========================
        public async Task<IReadOnlyList<RolResponse>> FiltrarAsync(
            RolFiltroRequest request,
            CancellationToken ct = default)
        {
            var list = await _dataService.GetAllAsync(ct);

            var query = list.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.NombreRol))
                query = query.Where(x =>
                    x.Nombre.Contains(request.NombreRol, StringComparison.OrdinalIgnoreCase));

            if (request.Activo.HasValue)
                query = query.Where(x => x.Activo == request.Activo.Value);

            if (!string.IsNullOrWhiteSpace(request.Estado))
                query = query.Where(x => x.Estado == request.Estado);

            return RolBusinessMapper.ToResponseList(query.ToList());
        }
    }
}