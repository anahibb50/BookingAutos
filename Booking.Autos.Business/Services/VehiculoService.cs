using Booking.Autos.Business.DTOs.Vehiculo;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.Business.Validators;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models;
using Booking.Autos.DataManagement.Models.Vehiculos;
using Microservicio.Clientes.DataManagement.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Booking.Autos.Business.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoDataService _vehiculoDataService;

        public VehiculoService(IVehiculoDataService vehiculoDataService)
        {
            _vehiculoDataService = vehiculoDataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<VehiculoResponse> CrearAsync(CrearVehiculoRequest request, CancellationToken cancellationToken = default)
        {
            var errors = VehiculoValidator.ValidarCreacion(request);

            if (errors.Any())
                throw new ValidationException(errors.ToList());

            var existente = await _vehiculoDataService.GetByPlacaAsync(request.Placa, cancellationToken);

            if (existente is not null)
                throw new ValidationException(errors.ToList());

            var dataModel = VehiculoBusinessMapper.ToDataModel(request);

            // 🔥 lógica backend
            dataModel.Guid = Guid.NewGuid();
            dataModel.Estado = "DIS"; // Disponible

            var creado = await _vehiculoDataService.CreateAsync(dataModel, cancellationToken);

            return VehiculoBusinessMapper.ToResponse(creado);
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<VehiculoResponse> ActualizarAsync(ActualizarVehiculoRequest request, CancellationToken cancellationToken = default)
        {
            var errors = VehiculoValidator.ValidarActualizacion(request);

            if (errors.Any())
                throw new ValidationException(errors.ToList());

            var existente = await _vehiculoDataService.GetByIdAsync(request.Id, cancellationToken);

            if (existente is null)
                throw new NotFoundException("No se encontró el vehículo solicitado.");

            var porPlaca = await _vehiculoDataService.GetByPlacaAsync(request.Placa, cancellationToken);

            if (porPlaca is not null && porPlaca.Id != request.Id)
                throw new ValidationException(errors.ToList());

            var dataModel = VehiculoBusinessMapper.ToDataModel(request);

            // 🔥 mantener datos originales
            dataModel.Guid = existente.Guid;

            var actualizado = await _vehiculoDataService.UpdateAsync(dataModel, cancellationToken);

            if (actualizado is null)
                throw new NotFoundException("No se pudo actualizar el vehículo.");

            return VehiculoBusinessMapper.ToResponse(actualizado);
        }

        // =========================
        // OBTENER
        // =========================
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

            return vehiculo is null ? null : VehiculoBusinessMapper.ToResponse(vehiculo);
        }

        public async Task<IReadOnlyList<VehiculoResponse>> ListarAsync(CancellationToken cancellationToken = default)
        {
            var vehiculos = await _vehiculoDataService.GetAllAsync(cancellationToken);

            return vehiculos.Select(VehiculoBusinessMapper.ToResponse).ToList();
        }

        // =========================
        // BUSCAR (PAGINADO)
        // =========================
        public async Task<DataPagedResult<VehiculoResponse>> BuscarAsync(VehiculoFiltroRequest request, CancellationToken cancellationToken = default)
        {
            var filtro = new VehiculoFiltroDataModel
            {
                IdMarca = request.IdMarca,
                IdCategoria = request.IdCategoria,
                PrecioMin = request.PrecioMin,
                PrecioMax = request.PrecioMax,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var result = await _vehiculoDataService.BuscarAsync(filtro, cancellationToken);

            return new DataPagedResult<VehiculoResponse>
            {
                Items = result.Items.Select(VehiculoBusinessMapper.ToResponse).ToList(),
                Page = result.Page,
                PageSize = result.PageSize
                
            };
        }

        // =========================
        // FILTROS
        // =========================
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
            var list = await _vehiculoDataService.GetByRangoPrecioAsync(min, max, cancellationToken);
            return list.Select(VehiculoBusinessMapper.ToResponse).ToList();
        }

        // =========================
        // DISPONIBILIDAD
        // =========================
        public async Task<bool> VerificarDisponibilidadAsync(int idVehiculo, DateTime fechaInicio, DateTime fechaFin, CancellationToken cancellationToken = default)
        {
            return await _vehiculoDataService.IsDisponibleAsync(idVehiculo, fechaInicio, fechaFin, cancellationToken);
        }

        // =========================
        // OPERACIONES ESPECIALES
        // =========================
        public async Task<bool> ActualizarKilometrajeAsync(int id, int nuevoKilometraje, CancellationToken cancellationToken = default)
        {
            var errors = VehiculoValidator.ValidarActualizacion(new ActualizarVehiculoRequest { Id = id, KilometrajeActual = nuevoKilometraje });
            if (nuevoKilometraje < 0)
                throw new ValidationException(errors.ToList());

            return await _vehiculoDataService.UpdateKilometrajeAsync(id, nuevoKilometraje, cancellationToken);
        }

        public async Task<bool> ActualizarEstadoAsync(int id, string estado, CancellationToken cancellationToken = default)
        {
            return await _vehiculoDataService.UpdateEstadoAsync(id, estado, cancellationToken);
        }

        // =========================
        // VALIDACIONES
        // =========================
        public async Task<bool> ExistePorPlacaAsync(string placa, CancellationToken cancellationToken = default)
        {
            return await _vehiculoDataService.ExistsByPlacaAsync(placa, cancellationToken);
        }

        // =========================
        // ELIMINAR
        // =========================
        public async Task EliminarLogicoAsync(int id, string usuario, CancellationToken cancellationToken = default)
        {
            var eliminado = await _vehiculoDataService.DeleteAsync(id, cancellationToken);

            if (!eliminado)
                throw new NotFoundException("No se encontró el vehículo para eliminar.");
        }
    }
}