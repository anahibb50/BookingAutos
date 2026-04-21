using Booking.Autos.Business.DTOs.Factura;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Facturas;
using Booking.Autos.DataManagement.Common;

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

        // =========================
        // CREAR
        // =========================
        public async Task<FacturaResponse> CrearAsync(
            CrearFacturaRequest request,
            CancellationToken ct = default)
        {
            var reserva = await _reservaDataService
                .GetByIdAsync(request.IdReserva, ct);

            if (reserva is null)
                throw new NotFoundException("Reserva", request.IdReserva);

            var existente = await _dataService
                .GetByReservaAsync(request.IdReserva, ct);

            if (existente != null)
                throw new ValidationException(new List<string>
                {
                    "La reserva ya tiene una factura."
                });

            var model = FacturaBusinessMapper.ToDataModel(request);

            // 🔥 completar valores desde reserva
            model.Subtotal = reserva.Subtotal;
            model.Iva = reserva.Iva;
            model.Total = reserva.Total;

            var creada = await _dataService.CreateAsync(model, ct);

            return FacturaBusinessMapper.ToResponse(creada);
        }

        // =========================
        // OBTENER POR ID
        // =========================
        public async Task<FacturaResponse> ObtenerPorIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var factura = await _dataService.GetByIdAsync(id, ct);

            if (factura is null)
                throw new NotFoundException("Factura", id);

            return FacturaBusinessMapper.ToResponse(factura);
        }

        // =========================
        // LISTAR
        // =========================
        public async Task<IReadOnlyList<FacturaResponse>> ListarAsync(
            CancellationToken ct = default)
        {
            var list = await _dataService.GetAllAsync(ct);

            return FacturaBusinessMapper.ToResponseList(list);
        }

        // =========================
        // POR CLIENTE
        // =========================
        public async Task<IReadOnlyList<FacturaResponse>> ObtenerPorClienteAsync(
            int idCliente,
            CancellationToken ct = default)
        {
            var list = await _dataService.GetByClienteAsync(idCliente, ct);

            return FacturaBusinessMapper.ToResponseList(list);
        }

        // =========================
        // POR RESERVA
        // =========================
        public async Task<FacturaResponse?> ObtenerPorReservaAsync(
            int idReserva,
            CancellationToken ct = default)
        {
            var factura = await _dataService.GetByReservaAsync(idReserva, ct);

            return factura is null
                ? null
                : FacturaBusinessMapper.ToResponse(factura);
        }

        // =========================
        // BUSCAR (PAGINADO)
        // =========================
        public async Task<DataPagedResult<FacturaResponse>> BuscarAsync(
            FacturaFiltroRequest request,
            CancellationToken ct = default)
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

        // =========================
        // APROBAR
        // =========================
        public async Task<bool> AprobarAsync(
            int id,
            CancellationToken ct = default)
        {
            var factura = await _dataService.GetByIdAsync(id, ct);

            if (factura is null)
                throw new NotFoundException("Factura", id);

            if (factura.Estado == "APR")
                throw new ValidationException(new List<string>
                {
                    "La factura ya está aprobada."
                });

            return await _dataService.AprobarAsync(id, ct);
        }

        // =========================
        // ANULAR
        // =========================
        public async Task<bool> AnularAsync(
            int id,
            string motivo,
            CancellationToken ct = default)
        {
            var factura = await _dataService.GetByIdAsync(id, ct);

            if (factura is null)
                throw new NotFoundException("Factura", id);

            if (factura.Estado == "ANU")
                throw new ValidationException(new List<string>
                {
                    "La factura ya está anulada."
                });

            return await _dataService.AnularAsync(id, motivo, ct);
        }
    }
}