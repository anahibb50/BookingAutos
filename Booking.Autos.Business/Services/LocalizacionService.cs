using Booking.Autos.Business.DTOs.Localizacion;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.Business.Validators;
using Booking.Autos.DataManagement.Interfaces;

namespace Booking.Autos.Business.Services
{
    public class LocalizacionService : ILocalizacionService
    {
        private readonly ILocalizacionDataService _dataService;

        public LocalizacionService(ILocalizacionDataService dataService)
        {
            _dataService = dataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<LocalizacionResponse> CrearAsync(
            CrearLocalizacionRequest request,
            CancellationToken ct = default)
        {
            var errors = LocalizacionValidator.ValidarCreacion(request);

            if (errors.Any())
                throw new ValidationException(errors.ToList());

            // 🔥 validar duplicado en ciudad
            var existe = await _dataService
                .ExistsByNombreEnCiudadAsync(request.Nombre, request.IdCiudad, ct);

            if (existe)
                throw new ValidationException(new List<string>
                {
                    "Ya existe una localización con ese nombre en la ciudad."
                });

            var model = LocalizacionBusinessMapper.ToDataModel(request);

            // 🔥 completar datos del modelo
            model.Codigo = $"LOC-{Guid.NewGuid().ToString()[..6].ToUpper()}";
            model.CreadoPorUsuario = "SYSTEM";
            model.OrigenRegistro = "API";

            var creado = await _dataService.CreateAsync(model, ct);

            return LocalizacionBusinessMapper.ToResponse(creado);
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<LocalizacionResponse> ActualizarAsync(
            ActualizarLocalizacionRequest request,
            CancellationToken ct = default)
        {
            request.Nombre = request.Nombre?.Trim();

            var errors = LocalizacionValidator.ValidarActualizacion(request);

            if (errors.Any())
                throw new ValidationException(errors.ToList());

            var existente = await _dataService.GetByIdAsync(request.Id, ct);

            if (existente is null)
                throw new NotFoundException("Localizacion", request.Id);

            // 🔥 validar duplicado (si cambió nombre o ciudad)
            if (!string.Equals(existente.Nombre, request.Nombre, StringComparison.OrdinalIgnoreCase) ||
                existente.IdCiudad != request.IdCiudad)
            {
                var existe = await _dataService
                    .ExistsByNombreEnCiudadAsync(request.Nombre, request.IdCiudad, ct);

                if (existe)
                    throw new ValidationException(new List<string>
                    {
                        "Ya existe una localización con ese nombre en la ciudad."
                    });
            }

            var model = LocalizacionBusinessMapper.ToDataModel(request);

            // 🔥 conservar datos críticos
            model.Guid = existente.Guid;
            model.Codigo = existente.Codigo;
            model.FechaRegistroUtc = existente.FechaRegistroUtc;
            model.CreadoPorUsuario = existente.CreadoPorUsuario;
            model.OrigenRegistro = existente.OrigenRegistro;
            model.EsEliminado = existente.EsEliminado;
            model.Estado = existente.Estado;

            // 🔥 auditoría
            model.ModificadoPorUsuario = "SYSTEM";
            model.ModificadoDesdeIp = "127.0.0.1";

            var actualizado = await _dataService.UpdateAsync(model, ct);

            return LocalizacionBusinessMapper.ToResponse(actualizado);
        }

        // =========================
        // INHABILITAR
        // =========================
        public async Task InhabilitarAsync(
            int id,
            string usuario,
            CancellationToken ct = default)
        {
            var existente = await _dataService.GetByIdAsync(id, ct);

            if (existente is null)
                throw new NotFoundException("Localizacion", id);

            var errors = LocalizacionValidator.ValidarInhabilitacion(existente.Estado);

            if (errors.Any())
                throw new ValidationException(errors.ToList());

            existente.Estado = "INA";
            existente.FechaInhabilitacionUtc = DateTime.UtcNow;
            existente.ModificadoPorUsuario = usuario;

            await _dataService.UpdateAsync(existente, ct);
        }

        // =========================
        // OBTENER POR ID
        // =========================
        public async Task<LocalizacionResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var model = await _dataService.GetByIdAsync(id, ct);

            if (model is null)
                throw new NotFoundException("Localizacion", id);

            return LocalizacionBusinessMapper.ToResponse(model);
        }

        // =========================
        // LISTAR
        // =========================
        public async Task<IReadOnlyList<LocalizacionResponse>> ListarAsync(
            CancellationToken ct = default)
        {
            var list = await _dataService.GetAllAsync(ct);

            return LocalizacionBusinessMapper.ToResponseList(list);
        }

        // =========================
        // POR CIUDAD
        // =========================
        public async Task<IReadOnlyList<LocalizacionResponse>> ObtenerPorCiudadAsync(
            int idCiudad,
            CancellationToken ct = default)
        {
            var list = await _dataService.GetByCiudadAsync(idCiudad, ct);

            return LocalizacionBusinessMapper.ToResponseList(list);
        }

        // =========================
        // POR NOMBRE
        // =========================
        public async Task<LocalizacionResponse?> ObtenerPorNombreAsync(
            string nombre,
            CancellationToken ct = default)
        {
            var model = await _dataService.GetByNombreAsync(nombre, ct);

            return model is null
                ? null
                : LocalizacionBusinessMapper.ToResponse(model);
        }

        // =========================
        // VALIDACIÓN
        // =========================
        public async Task<bool> ExistePorNombreEnCiudadAsync(
            string nombre,
            int idCiudad,
            CancellationToken ct = default)
        {
            return await _dataService
                .ExistsByNombreEnCiudadAsync(nombre, idCiudad, ct);
        }
    }
}