using Booking.Autos.Business.DTOs.Cliente;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.Business.Validators;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Clientes;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteDataService _clienteDataService;
        private readonly ICiudadDataService _ciudadDataService;

        public ClienteService(
            IClienteDataService clienteDataService,
            ICiudadDataService ciudadDataService)
        {
            _clienteDataService = clienteDataService;
            _ciudadDataService = ciudadDataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<ClienteResponse> CrearAsync(
            CrearClienteRequest request,
            CancellationToken cancellationToken = default)
        {
            var errors = ClienteValidator.ValidarCreacion(request);

            if (errors.Any())
                throw new ValidationException(errors.ToList());

            var existe = await _clienteDataService
                .ExistsByIdentificacionAsync(request.Identificacion, cancellationToken);

            if (existe)
                throw new ValidationException(new List<string> { "Ya existe un cliente con esa identificación." });

            var ciudad = await _ciudadDataService.GetByIdAsync(request.IdCiudad, cancellationToken);

            if (ciudad is null)
                throw new ValidationException(new List<string> { $"No existe la ciudad con id {request.IdCiudad}." });

            var model = ClienteBusinessMapper.ToDataModel(request);
            model.CreadoPorUsuario = "SYSTEM";
            model.ServicioOrigen = "API";

            var creado = await _clienteDataService
                .CreateAsync(model, cancellationToken);

            return ClienteBusinessMapper.ToResponse(creado);
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<ClienteResponse> ActualizarAsync(
            ActualizarClienteRequest request,
            CancellationToken cancellationToken = default)
        {
            var errors = ClienteValidator.ValidarActualizacion(request);

            if (errors.Any())
                throw new ValidationException(errors.ToList());

            var existente = await _clienteDataService
                .GetByIdAsync(request.Id, cancellationToken);

            if (existente is null)
                throw new NotFoundException("Cliente", request.Id);

            var existe = await _clienteDataService
                .ExistsByIdentificacionAsync(request.Identificacion, cancellationToken);

            if (existe && existente.Identificacion != request.Identificacion)
                throw new ValidationException(new List<string> { "Ya existe otro cliente con esa identificación." });

            var ciudad = await _ciudadDataService.GetByIdAsync(request.IdCiudad, cancellationToken);

            if (ciudad is null)
                throw new ValidationException(new List<string> { $"No existe la ciudad con id {request.IdCiudad}." });

            var model = ClienteBusinessMapper.ToDataModel(request);

            // 🔥 conservar datos importantes
            model.Guid = existente.Guid;
            model.FechaRegistroUtc = existente.FechaRegistroUtc;
            model.CreadoPorUsuario = existente.CreadoPorUsuario;
            model.ServicioOrigen = existente.ServicioOrigen;
            model.EsEliminado = existente.EsEliminado;
            model.ModificadoPorUsuario = "SYSTEM";
            model.ModificacionIp = "127.0.0.1";

            var actualizado = await _clienteDataService
                .UpdateAsync(model, cancellationToken);

            return ClienteBusinessMapper.ToResponse(actualizado);
        }

        // =========================
        // ELIMINACIÓN LÓGICA
        // =========================
        public async Task EliminarLogicoAsync(
            int id,
            string usuario,
            CancellationToken cancellationToken = default)
        {
            var existente = await _clienteDataService
                .GetByIdAsync(id, cancellationToken);

            if (existente is null)
                throw new NotFoundException("Cliente", id);

            existente.EsEliminado = true;
            existente.FechaEliminacion = DateTime.UtcNow;
            existente.ModificadoPorUsuario = usuario;
            existente.ModificacionIp = "127.0.0.1";

            await _clienteDataService.UpdateAsync(existente, cancellationToken);
        }

        // =========================
        // OBTENER POR ID
        // =========================
        public async Task<ClienteResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var cliente = await _clienteDataService
                .GetByIdAsync(id, cancellationToken);

            if (cliente is null)
                throw new NotFoundException("Cliente", id);

            return ClienteBusinessMapper.ToResponse(cliente);
        }

        // =========================
        // OBTENER POR IDENTIFICACIÓN
        // =========================
        public async Task<ClienteResponse?> ObtenerPorIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default)
        {
            var cliente = await _clienteDataService
                .GetByIdentificacionAsync(identificacion, cancellationToken);

            if (cliente == null)
                throw new NotFoundException("Cliente no encontrado", identificacion);

            return ClienteBusinessMapper.ToResponse(cliente);
        }

        // =========================
        // LISTAR
        // =========================
        public async Task<IReadOnlyList<ClienteResponse>> ListarAsync(
            CancellationToken cancellationToken = default)
        {
            var clientes = await _clienteDataService
                .GetAllAsync(cancellationToken);

            return clientes
                .Select(ClienteBusinessMapper.ToResponse)
                .ToList();
        }

        // =========================
        // BUSCAR (PAGINADO 🔥)
        // =========================
        public async Task<DataPagedResult<ClienteResponse>> BuscarAsync(
            ClienteFiltroRequest request,
            CancellationToken cancellationToken = default)
        {
            var filtro = new ClienteFiltroDataModel
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Identificacion = request.Identificacion,
                TipoIdentificacion = request.TipoIdentificacion,
                IdCiudad = request.IdCiudad,
                Estado = request.Estado,
                Email = request.Email,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var result = await _clienteDataService
                .BuscarAsync(filtro, cancellationToken);

            return new DataPagedResult<ClienteResponse>
            {
                Items = result.Items.Select(ClienteBusinessMapper.ToResponse).ToList(),
                TotalRecords = result.TotalRecords,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }

        // =========================
        // VALIDACIÓN
        // =========================
        public async Task<bool> ExistePorIdentificacionAsync(
            string identificacion,
            CancellationToken cancellationToken = default)
        {
            return await _clienteDataService
                .ExistsByIdentificacionAsync(identificacion, cancellationToken);
        }
    }
}
