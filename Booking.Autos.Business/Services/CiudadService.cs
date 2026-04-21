using Booking.Autos.Business.DTOs.Catalogos.Ciudad;
using Booking.Autos.Business.DTOs.Ciudad;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Interfaces;

namespace Booking.Autos.Business.Services
{
    public class CiudadService : ICiudadService
    {
        private readonly ICiudadDataService _ciudadDataService;

        public CiudadService(ICiudadDataService ciudadDataService)
        {
            _ciudadDataService = ciudadDataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<CiudadResponse> CrearAsync(
            CrearCiudadRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ValidationException(new List<string> { "El nombre es obligatorio." });

            // 🔥 validar duplicado por país
            var existe = await _ciudadDataService
                .ExistsByNombreAsync(request.Nombre, request.IdPais, cancellationToken);

            if (existe)
                throw new ValidationException(new List<string> { "Ya existe una ciudad con ese nombre en el país." });

            var dataModel = CiudadBusinessMapper.ToDataModel(request);

            var creado = await _ciudadDataService
                .CreateAsync(dataModel, cancellationToken);

            return CiudadBusinessMapper.ToResponse(creado);
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<CiudadResponse> ActualizarAsync(
            ActualizarCiudadRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request.Id <= 0)
                throw new ValidationException(new List<string> { "Id inválido." });

            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ValidationException(new List<string> { "El nombre es obligatorio." });

            var existente = await _ciudadDataService
                .GetByIdAsync(request.Id, cancellationToken);

            if (existente is null)
                throw new NotFoundException("Ciudad", request.Id);

            // 🔥 validar duplicado
            var existeNombre = await _ciudadDataService
                .ExistsByNombreAsync(request.Nombre, request.IdPais, cancellationToken);

            if (existeNombre &&
                (existente.Nombre != request.Nombre || existente.IdPais != request.IdPais))
            {
                throw new ValidationException(new List<string> { "Ya existe otra ciudad con ese nombre en el país." });
            }

            var dataModel = CiudadBusinessMapper.ToDataModel(request);

            // 🔥 conservar datos importantes
            dataModel.Guid = existente.Guid;
            dataModel.FechaCreacion = existente.FechaCreacion;
            dataModel.EsEliminado = existente.EsEliminado;

            var actualizado = await _ciudadDataService
                .UpdateAsync(dataModel, cancellationToken);

            return CiudadBusinessMapper.ToResponse(actualizado);
        }

        // =========================
        // ELIMINACIÓN LÓGICA
        // =========================
        public async Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default)
        {
            var existente = await _ciudadDataService
                .GetByIdAsync(id, cancellationToken);

            if (existente is null)
                throw new NotFoundException("Ciudad", id);

            existente.EsEliminado = true;
            existente.FechaEliminacion = DateTime.UtcNow;

            await _ciudadDataService.UpdateAsync(existente, cancellationToken);
        }

        // =========================
        // OBTENER POR ID
        // =========================
        public async Task<CiudadResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var ciudad = await _ciudadDataService
                .GetByIdAsync(id, cancellationToken);

            if (ciudad is null)
                throw new NotFoundException("Ciudad", id);

            return CiudadBusinessMapper.ToResponse(ciudad);
        }

        // =========================
        // LISTAR
        // =========================
        public async Task<IReadOnlyList<CiudadResponse>> ListarAsync(
            CancellationToken cancellationToken = default)
        {
            var ciudades = await _ciudadDataService
                .GetAllAsync(cancellationToken);

            return ciudades
                .Select(CiudadBusinessMapper.ToResponse)
                .ToList();
        }

        // =========================
        // POR PAÍS 🔥
        // =========================
        public async Task<IReadOnlyList<CiudadResponse>> ObtenerPorPaisAsync(
            int idPais,
            CancellationToken cancellationToken = default)
        {
            var ciudades = await _ciudadDataService
                .GetByPaisAsync(idPais, cancellationToken);

            return ciudades
                .Select(CiudadBusinessMapper.ToResponse)
                .ToList();
        }

        // =========================
        // VALIDACIÓN
        // =========================
        public async Task<bool> ExistePorNombreAsync(
            string nombre,
            int idPais,
            CancellationToken cancellationToken = default)
        {
            return await _ciudadDataService
                .ExistsByNombreAsync(nombre, idPais, cancellationToken);
        }
    }
}