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
        private readonly IPaisDataService _paisDataService;

        public CiudadService(
            ICiudadDataService ciudadDataService,
            IPaisDataService paisDataService)
        {
            _ciudadDataService = ciudadDataService;
            _paisDataService = paisDataService;
        }

        public async Task<CiudadResponse> CrearAsync(
            CrearCiudadRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ValidationException(new List<string> { "El nombre es obligatorio." });

            if (string.IsNullOrWhiteSpace(request.CodigoPostal))
                throw new ValidationException(new List<string> { "El código postal es obligatorio." });

            var pais = await _paisDataService.GetByIdAsync(request.IdPais, cancellationToken);
            if (pais is null)
                throw new ValidationException(new List<string> { $"No existe el país con id {request.IdPais}." });

            var existe = await _ciudadDataService
                .ExistsByNombreAsync(request.Nombre, request.IdPais, cancellationToken);

            if (existe)
                throw new ValidationException(new List<string> { "Ya existe una ciudad con ese nombre en el país." });

            var dataModel = CiudadBusinessMapper.ToDataModel(request);

            var creado = await _ciudadDataService.CreateAsync(dataModel, cancellationToken);

            return CiudadBusinessMapper.ToResponse(creado);
        }

        public async Task<CiudadResponse> ActualizarAsync(
            ActualizarCiudadRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request.Id <= 0)
                throw new ValidationException(new List<string> { "Id inválido." });

            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ValidationException(new List<string> { "El nombre es obligatorio." });

            if (string.IsNullOrWhiteSpace(request.CodigoPostal))
                throw new ValidationException(new List<string> { "El código postal es obligatorio." });

            var existente = await _ciudadDataService.GetByIdAsync(request.Id, cancellationToken);

            if (existente is null)
                throw new NotFoundException("Ciudad", request.Id);

            var pais = await _paisDataService.GetByIdAsync(request.IdPais, cancellationToken);
            if (pais is null)
                throw new ValidationException(new List<string> { $"No existe el país con id {request.IdPais}." });

            var existeNombre = await _ciudadDataService
                .ExistsByNombreAsync(request.Nombre, request.IdPais, cancellationToken);

            if (existeNombre &&
                (existente.Nombre != request.Nombre || existente.IdPais != request.IdPais))
            {
                throw new ValidationException(new List<string> { "Ya existe otra ciudad con ese nombre en el país." });
            }

            var dataModel = CiudadBusinessMapper.ToDataModel(request);
            dataModel.Guid = existente.Guid;
            dataModel.FechaCreacion = existente.FechaCreacion;
            dataModel.EsEliminado = existente.EsEliminado;
            dataModel.Estado = existente.Estado;
            dataModel.OrigenRegistro = existente.OrigenRegistro;
            dataModel.FechaEliminacion = existente.FechaEliminacion;

            var actualizado = await _ciudadDataService.UpdateAsync(dataModel, cancellationToken);

            return CiudadBusinessMapper.ToResponse(actualizado);
        }

        public async Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default)
        {
            var existente = await _ciudadDataService.GetByIdAsync(id, cancellationToken);

            if (existente is null)
                throw new NotFoundException("Ciudad", id);

            existente.EsEliminado = true;
            existente.FechaEliminacion = DateTime.UtcNow;
            existente.Estado = "INA";

            await _ciudadDataService.UpdateAsync(existente, cancellationToken);
        }

        public async Task<CiudadResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var ciudad = await _ciudadDataService.GetByIdAsync(id, cancellationToken);

            if (ciudad is null)
                throw new NotFoundException("Ciudad", id);

            return CiudadBusinessMapper.ToResponse(ciudad);
        }

        public async Task<IReadOnlyList<CiudadResponse>> ListarAsync(
            CancellationToken cancellationToken = default)
        {
            var ciudades = await _ciudadDataService.GetAllAsync(cancellationToken);

            return ciudades.Select(CiudadBusinessMapper.ToResponse).ToList();
        }

        public async Task<IReadOnlyList<CiudadResponse>> ObtenerPorPaisAsync(
            int idPais,
            CancellationToken cancellationToken = default)
        {
            var ciudades = await _ciudadDataService.GetByPaisAsync(idPais, cancellationToken);

            return ciudades.Select(CiudadBusinessMapper.ToResponse).ToList();
        }

        public async Task<bool> ExistePorNombreAsync(
            string nombre,
            int idPais,
            CancellationToken cancellationToken = default)
        {
            return await _ciudadDataService.ExistsByNombreAsync(nombre, idPais, cancellationToken);
        }
    }
}
