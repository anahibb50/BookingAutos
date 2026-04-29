using Booking.Autos.Business.DTOs.Factura;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.Business.Validators;
using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Facturas;

namespace Booking.Autos.Business.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaDataService _dataService;
        private readonly IReservaDataService _reservaDataService;

        public FacturaService(
            IFacturaDataService dataService,
            IReservaDataService reservaDataService)
        {
            _dataService = dataService;
            _reservaDataService = reservaDataService;
        }

        public async Task<FacturaResponse> CrearAsync(CrearFacturaRequest request, CancellationToken ct = default)
        {
            var errors = FacturaValidator.ValidarCreacion(request);
            if (errors.Any())
                throw new ValidationException(errors.ToList());

            var reserva = await _reservaDataService.GetByIdAsync(request.IdReserva, ct);
            if (reserva is null)
                throw new NotFoundException("Reserva", request.IdReserva);

            var existente = await _dataService.GetByReservaAsync(request.IdReserva, ct);
            if (existente != null)
                throw new ValidationException(new List<string> { "La reserva ya tiene una factura." });

            var model = FacturaBusinessMapper.ToDataModel(request);
            model.IdCliente = reserva.IdCliente;
            model.Subtotal = reserva.Subtotal;
            model.Iva = reserva.Iva;
            model.Total = reserva.Total;

            if (model.Subtotal < 0 || model.Iva < 0 || model.Total < 0)
                throw new ValidationException(new List<string> { "Los totales de la factura no pueden ser negativos." });

            var creada = await _dataService.CreateAsync(model, ct);
            return FacturaBusinessMapper.ToResponse(creada);
        }

        public async Task<FacturaResponse> ActualizarAsync(ActualizarFacturaRequest request, CancellationToken ct = default)
        {
            var errors = FacturaValidator.ValidarActualizacion(request);
            if (errors.Any())
                throw new ValidationException(errors.ToList());

            var existente = await _dataService.GetByIdAsync(request.Id, ct);
            if (existente is null)
                throw new NotFoundException("Factura", request.Id);

            if (!string.Equals(existente.Estado, "ABI", StringComparison.OrdinalIgnoreCase))
                throw new ValidationException(new List<string> { "Solo se pueden actualizar facturas en estado ABI." });

            var model = FacturaBusinessMapper.ToDataModel(request);
            model.Guid = existente.Guid;
            model.IdReserva = existente.IdReserva;
            model.IdCliente = existente.IdCliente;
            model.Subtotal = existente.Subtotal;
            model.Iva = existente.Iva;
            model.Total = existente.Total;
            model.Estado = existente.Estado;
            model.FechaCreacion = existente.FechaCreacion;
            model.FechaAprobacion = existente.FechaAprobacion;
            model.FechaAnulacion = existente.FechaAnulacion;
            model.MotivoAnulacion = existente.MotivoAnulacion;
            model.EsEliminado = existente.EsEliminado;
            model.FechaEliminacion = existente.FechaEliminacion;
            model.Origen = existente.Origen;

            var actualizada = await _dataService.UpdateAsync(model, ct);
            return FacturaBusinessMapper.ToResponse(actualizada);
        }

        public async Task<FacturaResponse> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            var factura = await _dataService.GetByIdAsync(id, ct);
            if (factura is null)
                throw new NotFoundException("Factura", id);

            return FacturaBusinessMapper.ToResponse(factura);
        }

        public async Task<IReadOnlyList<FacturaResponse>> ListarAsync(CancellationToken ct = default)
        {
            var list = await _dataService.GetAllAsync(ct);
            return FacturaBusinessMapper.ToResponseList(list);
        }

        public async Task<IReadOnlyList<FacturaResponse>> ObtenerPorClienteAsync(int idCliente, CancellationToken ct = default)
        {
            var list = await _dataService.GetByClienteAsync(idCliente, ct);
            return FacturaBusinessMapper.ToResponseList(list);
        }

        public async Task<FacturaResponse?> ObtenerPorReservaAsync(int idReserva, CancellationToken ct = default)
        {
            var factura = await _dataService.GetByReservaAsync(idReserva, ct);
            return factura is null ? null : FacturaBusinessMapper.ToResponse(factura);
        }

        public async Task<DataPagedResult<FacturaResponse>> BuscarAsync(FacturaFiltroRequest request, CancellationToken ct = default)
        {
            var filtro = new FacturaFiltroDataModel
            {
                IdCliente = request.IdCliente,
                IdReserva = request.IdReserva,
                Estado = request.Estado,
                FechaCreacionDesde = request.FechaCreacionDesde,
                FechaCreacionHasta = request.FechaCreacionHasta,
                TotalMin = request.TotalMin,
                TotalMax = request.TotalMax,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var result = await _dataService.BuscarAsync(filtro, ct);

            return new DataPagedResult<FacturaResponse>
            {
                Items = FacturaBusinessMapper.ToResponseList(result.Items),
                TotalRecords = result.TotalRecords,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }

        public async Task<bool> AprobarAsync(int id, CancellationToken ct = default)
        {
            var factura = await _dataService.GetByIdAsync(id, ct);

            if (factura is null)
                throw new NotFoundException("Factura", id);

            if (!string.Equals(factura.Estado, "ABI", StringComparison.OrdinalIgnoreCase))
                throw new ValidationException(new List<string> { "Solo se pueden aprobar facturas en estado ABI." });

            return await _dataService.AprobarAsync(id, ct);
        }

        public async Task<bool> AnularAsync(int id, string motivo, CancellationToken ct = default)
        {
            var factura = await _dataService.GetByIdAsync(id, ct);

            if (factura is null)
                throw new NotFoundException("Factura", id);

            if (!string.Equals(factura.Estado, "ABI", StringComparison.OrdinalIgnoreCase))
                throw new ValidationException(new List<string> { "Solo se pueden anular facturas en estado ABI." });

            return await _dataService.AnularAsync(id, motivo, ct);
        }
    }
}
