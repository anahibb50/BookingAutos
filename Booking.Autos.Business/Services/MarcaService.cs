using Booking.Autos.Business.DTOs.Catalogos.Marca;
using Booking.Autos.Business.DTOs.Marca;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Interfaces;

namespace Booking.Autos.Business.Services
{
    public class MarcaService : IMarcaService
    {
        private readonly IMarcaDataService _dataService;

        public MarcaService(IMarcaDataService dataService)
        {
            _dataService = dataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<MarcaResponse> CrearAsync(
            CrearMarcaRequest request,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ValidationException(new List<string>
                {
                    "El nombre es obligatorio."
                });

            var existe = await _dataService
                .ExistsByNombreAsync(request.Nombre, ct);

            if (existe)
                throw new ValidationException(new List<string>
                {
                    "Ya existe una marca con ese nombre."
                });

            var model = MarcaBusinessMapper.ToDataModel(request);

            var creada = await _dataService.CreateAsync(model, ct);

            return MarcaBusinessMapper.ToResponse(creada);
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<MarcaResponse> ActualizarAsync(
            ActualizarMarcaRequest request,
            CancellationToken ct = default)
        {
            if (request.Id <= 0)
                throw new ValidationException(new List<string>
                {
                    "Id inválido."
                });

            var existente = await _dataService.GetByIdAsync(request.Id, ct);

            if (existente is null)
                throw new NotFoundException("Marca", request.Id);

            // 🔥 validar duplicado si cambia nombre
            if (!string.Equals(existente.Nombre, request.Nombre, StringComparison.OrdinalIgnoreCase))
            {
                var existe = await _dataService
                    .ExistsByNombreAsync(request.Nombre, ct);

                if (existe)
                    throw new ValidationException(new List<string>
                    {
                        "Ya existe una marca con ese nombre."
                    });
            }

            var model = MarcaBusinessMapper.ToDataModel(request);

            // 🔥 conservar datos importantes
            model.Guid = existente.Guid;
            model.FechaCreacion = existente.FechaCreacion;
            model.EsEliminado = existente.EsEliminado;

            var actualizado = await _dataService.UpdateAsync(model, ct);

            return MarcaBusinessMapper.ToResponse(actualizado);
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
                throw new NotFoundException("Marca", id);

            existente.EsEliminado = true;
            existente.FechaEliminacion = DateTime.UtcNow;

            await _dataService.UpdateAsync(existente, ct);
        }

        // =========================
        // OBTENER POR ID
        // =========================
        public async Task<MarcaResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var model = await _dataService.GetByIdAsync(id, ct);

            if (model is null)
                throw new NotFoundException("Marca", id);

            return MarcaBusinessMapper.ToResponse(model);
        }

        // =========================
        // LISTAR
        // =========================
        public async Task<IReadOnlyList<MarcaResponse>> ListarAsync(
            CancellationToken ct = default)
        {
            var list = await _dataService.GetAllAsync(ct);

            return MarcaBusinessMapper.ToResponseList(list);
        }

        // =========================
        // POR NOMBRE
        // =========================
        public async Task<MarcaResponse?> ObtenerPorNombreAsync(
            string nombre,
            CancellationToken ct = default)
        {
            var list = await _dataService.GetAllAsync(ct);

            var model = list
                .FirstOrDefault(x => x.Nombre.ToLower() == nombre.ToLower());

            return model is null
                ? null
                : MarcaBusinessMapper.ToResponse(model);
        }

        // =========================
        // VALIDACIÓN
        // =========================
        public async Task<bool> ExistePorNombreAsync(
            string nombre,
            CancellationToken ct = default)
        {
            return await _dataService.ExistsByNombreAsync(nombre, ct);
        }
    }
}