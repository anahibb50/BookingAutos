using Booking.Autos.Business.DTOs.Vehiculo;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.Business.Validators;
using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Vehiculos;

namespace Booking.Autos.Business.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoDataService _vehiculoDataService;

        public VehiculoService(IVehiculoDataService vehiculoDataService)
        {
            _vehiculoDataService = vehiculoDataService;
        }

        public async Task<VehiculoResponse> CrearAsync(CrearVehiculoRequest request, CancellationToken ct = default)
        {
            var errors = VehiculoValidator.ValidarCreacion(request);

            if (errors.Any())
                throw new ValidationException(errors.ToList());

            var marcaExiste = await _vehiculoDataService.ExisteMarcaAsync(request.IdMarca, ct);
            var categoriaExiste = await _vehiculoDataService.ExisteCategoriaAsync(request.IdCategoria, ct);
            var localizacionExiste = await _vehiculoDataService.ExisteLocalizacionAsync(request.IdLocalizacion, ct);

            var fkErrors = VehiculoValidator.ValidarRelaciones(marcaExiste, categoriaExiste, localizacionExiste);
            if (fkErrors.Any())
                throw new ValidationException(fkErrors.ToList());

            var existente = await _vehiculoDataService.GetByPlacaAsync(request.Placa, ct);
            if (existente is not null)
                throw new ValidationException(new List<string> { "Ya existe un vehículo con esa placa." });

            var dataModel = VehiculoBusinessMapper.ToDataModel(request);
            dataModel.Guid = Guid.NewGuid();
            dataModel.CodigoInterno = $"VEH-{Guid.NewGuid().ToString("N")[..12].ToUpper()}";

            var creado = await _vehiculoDataService.CreateAsync(dataModel, ct);
            return VehiculoBusinessMapper.ToResponse(creado);
        }

        public async Task<VehiculoResponse> ActualizarAsync(ActualizarVehiculoRequest request, CancellationToken ct = default)
        {
            var errors = VehiculoValidator.ValidarActualizacion(request);

            if (errors.Any())
                throw new ValidationException(errors.ToList());

            var existente = await _vehiculoDataService.GetByIdAsync(request.Id, ct);

            if (existente is null)
                throw new NotFoundException("Vehiculo", request.Id);

            var porPlaca = await _vehiculoDataService.GetByPlacaAsync(request.Placa, ct);

            if (porPlaca is not null && porPlaca.Id != request.Id)
                throw new ValidationException(new List<string> { "La placa ya está en uso." });

            var dataModel = VehiculoBusinessMapper.ToDataModel(request);
            dataModel.Guid = existente.Guid;
            dataModel.CodigoInterno = existente.CodigoInterno;
            dataModel.FechaRegistroUtc = existente.FechaRegistroUtc;
            dataModel.EsEliminado = existente.EsEliminado;

            var actualizado = await _vehiculoDataService.UpdateAsync(dataModel, ct);
            return VehiculoBusinessMapper.ToResponse(actualizado);
        }

        public async Task<VehiculoResponse> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var vehiculo = await _vehiculoDataService.GetByIdAsync(id, cancellationToken);

            if (vehiculo is null)
                throw new NotFoundException("No se encontró el vehículo.");

            return VehiculoBusinessMapper.ToResponse(vehiculo);
        }

        public async Task<VehiculoResponse?> ObtenerPorPlacaAsync(string placa, CancellationToken cancellationToken = default)
        {
            var vehiculo = await _vehiculoDataService.GetByPlacaAsync(placa, cancellationToken);
            if (vehiculo is null)
                throw new NotFoundException("No se encontró el vehículo.");

            return VehiculoBusinessMapper.ToResponse(vehiculo);
        }

        public async Task<IReadOnlyList<VehiculoResponse>> ListarAsync(CancellationToken cancellationToken = default)
        {
            var vehiculos = await _vehiculoDataService.GetAllAsync(cancellationToken);
            return vehiculos.Select(VehiculoBusinessMapper.ToResponse).ToList();
        }

        public async Task<DataPagedResult<VehiculoResponse>> BuscarAsync(VehiculoFiltroRequest request, CancellationToken cancellationToken = default)
        {
            var filtro = new VehiculoFiltroDataModel
            {
                IdMarca = request.IdMarca,
                IdCategoria = request.IdCategoria,
                IdLocalizacion = request.IdLocalizacion,
                PrecioMin = request.PrecioMin,
                PrecioMax = request.PrecioMax,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                Estado = request.Estado,
                TipoTransmision = request.TipoTransmision,
                TipoCombustible = request.TipoCombustible,
                CapacidadMinPasajeros = request.CapacidadMinPasajeros,
                AireAcondicionado = request.AireAcondicionado,
                Modelo = request.Modelo,
                Placa = request.Placa,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var result = await _vehiculoDataService.BuscarAsync(filtro, cancellationToken);

            return new DataPagedResult<VehiculoResponse>
            {
                Items = result.Items.Select(VehiculoBusinessMapper.ToResponse).ToList(),
                Page = result.Page,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords
            };
        }

        public async Task<IReadOnlyList<VehiculoResponse>> ObtenerPorMarcaAsync(int idMarca, CancellationToken cancellationToken = default)
        {
            var list = await _vehiculoDataService.GetByMarcaAsync(idMarca, cancellationToken);
            return list.Select(VehiculoBusinessMapper.ToResponse).ToList();
        }

        public async Task<IReadOnlyList<VehiculoResponse>> ObtenerPorCategoriaAsync(int idCategoria, CancellationToken cancellationToken = default)
        {
            var list = await _vehiculoDataService.GetByCategoriaAsync(idCategoria, cancellationToken);
            return list.Select(VehiculoBusinessMapper.ToResponse).ToList();
        }

        public async Task<IReadOnlyList<VehiculoResponse>> ListarDisponiblesAsync(CancellationToken cancellationToken = default)
        {
            var list = await _vehiculoDataService.GetDisponiblesAsync(cancellationToken);
            return list.Select(VehiculoBusinessMapper.ToResponse).ToList();
        }

        public async Task<IReadOnlyList<VehiculoResponse>> ObtenerPorRangoPrecioAsync(decimal min, decimal max, CancellationToken cancellationToken = default)
        {
            if (min < 0 || max < 0)
                throw new ValidationException(new List<string> { "Los precios no pueden ser negativos." });

            var list = await _vehiculoDataService.GetByRangoPrecioAsync(min, max, cancellationToken);
            return list.Select(VehiculoBusinessMapper.ToResponse).ToList();
        }

        public async Task<bool> ActualizarKilometrajeAsync(int id, int nuevoKilometraje, CancellationToken ct = default)
        {
            if (nuevoKilometraje < 0)
                throw new ValidationException(new List<string> { "El kilometraje no puede ser negativo." });

            return await _vehiculoDataService.UpdateKilometrajeAsync(id, nuevoKilometraje, ct);
        }

        public async Task<bool> ActualizarEstadoAsync(int id, string estado, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new ValidationException(new List<string> { "El id del vehículo es inválido." });

            var errors = VehiculoValidator.ValidarEstado(estado);
            if (errors.Any())
                throw new ValidationException(errors.ToList());

            return await _vehiculoDataService.UpdateEstadoAsync(id, estado, cancellationToken);
        }

        public async Task<bool> ExistePorPlacaAsync(string placa, CancellationToken cancellationToken = default)
        {
            return await _vehiculoDataService.ExistsByPlacaAsync(placa, cancellationToken);
        }

        public async Task EliminarLogicoAsync(int id, string usuario, CancellationToken ct = default)
        {
            var existente = await _vehiculoDataService.GetByIdAsync(id, ct);

            if (existente is null)
                throw new NotFoundException("Vehiculo", id);

            existente.EsEliminado = true;
            existente.FechaInhabilitacionUtc = DateTime.UtcNow;
            existente.MotivoInhabilitacion = "Eliminado lógico";

            await _vehiculoDataService.UpdateAsync(existente, ct);
        }
    }
}
