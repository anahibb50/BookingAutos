using Booking.Autos.Business.DTOs.Cliente;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Clientes;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteDataService _clienteDataService;

        public ClienteService(IClienteDataService clienteDataService)
        {
            _clienteDataService = clienteDataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<ClienteResponse> CrearAsync(
            CrearClienteRequest request,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ValidationException(new List<string> { "El nombre es obligatorio." });

            if (string.IsNullOrWhiteSpace(request.Identificacion))
                throw new ValidationException(new List<string> { "La identificación es obligatoria." });

            var existe = await _clienteDataService
                .ExistsByIdentificacionAsync(request.Identificacion, cancellationToken);

            if (existe)
                throw new ValidationException(new List<string> { "Ya existe un cliente con esa identificación." });

            var model = ClienteBusinessMapper.ToDataModel(request);

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
            if (request.Id <= 0)
                throw new ValidationException(new List<string> { "Id inválido." });

            var existente = await _clienteDataService
                .GetByIdAsync(request.Id, cancellationToken);

            if (existente is null)
                throw new NotFoundException("Cliente", request.Id);

            var existe = await _clienteDataService
                .ExistsByIdentificacionAsync(request.Identificacion, cancellationToken);

            if (existe && existente.Identificacion != request.Identificacion)
                throw new ValidationException(new List<string> { "Ya existe otro cliente con esa identificación." });

            var model = ClienteBusinessMapper.ToDataModel(request);

            // 🔥 conservar datos importantes
            model.Guid = existente.Guid;
            model.FechaRegistroUtc = existente.FechaRegistroUtc;
            model.EsEliminado = existente.EsEliminado;

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

            if (cliente is null)
                return null;

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