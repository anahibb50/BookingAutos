using Booking.Autos.Business.DTOs.Extra;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Extras;

namespace Booking.Autos.Business.Services
{
    public class ExtraService : IExtraService
    {
        private readonly IExtraDataService _dataService;

        public ExtraService(IExtraDataService dataService)
        {
            _dataService = dataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<ExtraResponse> CrearAsync(
            CrearExtraRequest request,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ValidationException(new List<string> { "El nombre es obligatorio." });

            if (request.ValorFijo <= 0)
                throw new ValidationException(new List<string> { "El valor debe ser mayor a 0." });

            var model = ExtraBusinessMapper.ToDataModel(request);

            // 🔥 completar auditoría
            model.Codigo = $"EXT-{Guid.NewGuid().ToString()[..6].ToUpper()}";
            model.CreadoPorUsuario = "SYSTEM";
            model.OrigenRegistro = "API";

            var creado = await _dataService.CreateAsync(model, ct);

            return ExtraBusinessMapper.ToResponse(creado);
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<ExtraResponse> ActualizarAsync(
            ActualizarExtraRequest request,
            CancellationToken ct = default)
        {
            if (request.Id <= 0)
                throw new ValidationException(new List<string> { "Id inválido." });

            var existente = await _dataService.GetByIdAsync(request.Id, ct);

            if (existente is null)
                throw new NotFoundException("Extra", request.Id);

            var model = ExtraBusinessMapper.ToDataModel(request);

            // 🔥 conservar datos importantes
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

            return ExtraBusinessMapper.ToResponse(actualizado);
        }

        // =========================
        // ELIMINACIÓN LÓGICA
        // =========================
        public async Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken ct = default)
        {
            var existente = await _dataService.GetByIdAsync(id, ct);

            if (existente is null)
                throw new NotFoundException("Extra", id);

            existente.EsEliminado = true;
            existente.Estado = "INA";
            existente.FechaInhabilitacionUtc = DateTime.UtcNow;
            existente.MotivoInhabilitacion = "Eliminación lógica";
            existente.ModificadoPorUsuario = usuario;

            await _dataService.UpdateAsync(existente, ct);
        }

        // =========================
        // OBTENER POR ID
        // =========================
        public async Task<ExtraResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var extra = await _dataService.GetByIdAsync(id, ct);

            if (extra is null)
                throw new NotFoundException("Extra", id);

            return ExtraBusinessMapper.ToResponse(extra);
        }

        // =========================
        // LISTAR TODOS
        // =========================
        public async Task<IReadOnlyList<ExtraResponse>> ListarAsync(
            CancellationToken ct = default)
        {
            var list = await _dataService.GetAllAsync(ct);

            return list
                .Select(ExtraBusinessMapper.ToResponse)
                .ToList();
        }

        // =========================
        // LISTAR ACTIVOS
        // =========================
        public async Task<IReadOnlyList<ExtraResponse>> ListarActivosAsync(
            CancellationToken ct = default)
        {
            var list = await _dataService.GetActivosAsync(ct);

            return list
                .Select(ExtraBusinessMapper.ToResponse)
                .ToList();
        }

        // =========================
        // BUSCAR POR NOMBRE
        // =========================
        public async Task<IReadOnlyList<ExtraResponse>> ObtenerPorNombreAsync(
            string nombre,
            CancellationToken ct = default)
        {
            var list = await _dataService.GetByNombreAsync(nombre, ct);

            return list
                .Select(ExtraBusinessMapper.ToResponse)
                .ToList();
        }

        // =========================
        // ACTUALIZAR PRECIO 🔥
        // =========================
        public async Task<bool> ActualizarPrecioAsync(
            int id,
            decimal nuevoPrecio,
            CancellationToken ct = default)
        {
            if (nuevoPrecio <= 0)
                throw new ValidationException(new List<string> { "El precio debe ser mayor a 0." });

            var existe = await _dataService.GetByIdAsync(id, ct);

            if (existe is null)
                throw new NotFoundException("Extra", id);

            return await _dataService.UpdatePrecioAsync(id, nuevoPrecio, ct);
        }
    }
}
