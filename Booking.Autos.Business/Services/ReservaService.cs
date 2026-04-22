using Booking.Autos.Business.DTOs.ConductorReserva;
using Booking.Autos.Business.DTOs.Reserva;
using Booking.Autos.Business.DTOs.ReservaExtra;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.Business.Validators;
using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.Business.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaDataService _reservaDataService;
        private readonly IReservaExtraDataService _reservaExtraDataService;
        private readonly IConductorReservaService _conductorService;
        private readonly IVehiculoDataService _vehiculoDataService;
        private readonly IExtraDataService _extraDataService;
        private readonly IClienteDataService _clienteDataService;
        private readonly ILocalizacionDataService _localizacionDataService;
        private readonly IConductorDataService _conductorDataService;

        public ReservaService(
            IReservaDataService reservaDataService,
            IReservaExtraDataService reservaExtraDataService,
            IConductorReservaService conductorService,
            IExtraDataService extraDataService,
            IVehiculoDataService vehiculoDataService,
            IClienteDataService clienteDataService,                // 👈 nuevo
            ILocalizacionDataService localizacionDataService,
            IConductorDataService conductorDataService)
        {
            _reservaDataService = reservaDataService;
            _reservaExtraDataService = reservaExtraDataService;
            _extraDataService = extraDataService;
            _conductorService = conductorService;
            _vehiculoDataService = vehiculoDataService;
            _clienteDataService = clienteDataService;            // 👈 nuevo
            _localizacionDataService = localizacionDataService;
            _conductorDataService = conductorDataService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<ReservaResponse> CrearAsync(
            CrearReservaRequest request,
            CancellationToken ct = default)
        {
            var errors = ReservaValidator.ValidarCreacion(request);

            if (errors.Any())
                throw new ValidationException(errors.ToList());
            
            await ValidarReferenciasAsync(request, ct);

            var disponible = await _reservaDataService.IsVehiculoDisponibleAsync(
                request.IdVehiculo, request.FechaInicio, request.FechaFin, ct);

            if (!disponible)
                throw new BusinessException("Vehículo no disponible.");


            var model = ReservaBusinessMapper.ToDataModel(request);
            model.CreadoPorUsuario = "SYSTEM";
            model.ServicioOrigen = "API";


            var (subtotal, iva, total) = await CalcularTotalesAsync(
                request.IdVehiculo,
                request.CantidadDias,
                request.Extras,
                ct);

            model.Subtotal = subtotal;
            model.Iva = iva;
            model.Total = total;
            
            var reserva = await _reservaDataService.CreateAsync(model, ct);

            // 🔥 EXTRAS
            if (request.Extras != null)
            {
                foreach (var extra in request.Extras)
                {
                    await _reservaExtraDataService.AddAsync(new ReservaExtraDataModel
                    {
                        IdReserva = reserva.Id,
                        IdExtra = extra.IdExtra,
                        Cantidad = extra.Cantidad,
                        Estado = "ACT",
                        EsEliminado = false,
                        FechaCreacion = DateTime.UtcNow,
                        FechaActualizacion = DateTime.UtcNow
                    }, ct);
                }
            }

            // 🔥 CONDUCTORES
            if (request.Conductores != null)
            {
                foreach (var conductor in request.Conductores)
                {
                    await _conductorService.CrearAsync(reserva.Id, conductor, ct);
                }
            }


            return await ObtenerPorIdAsync(reserva.Id, ct);
        }

        // =========================
        // ACTUALIZAR
        // =========================
        public async Task<ReservaResponse> ActualizarAsync(
            ActualizarReservaRequest request,
            CancellationToken ct = default)
        {
            var existente = await _reservaDataService.GetByIdAsync(request.Id, ct);

            if (existente == null)
                throw new NotFoundException("Reserva", request.Id);

            var model = ReservaBusinessMapper.ToDataModel(request);

            model.IdCliente = existente.IdCliente;
            model.IdVehiculo = existente.IdVehiculo;

            var actualizado = await _reservaDataService.UpdateAsync(model, ct);

            // 🔥 actualizar conductores
            if (request.Conductores != null)
            {
                foreach (var c in request.Conductores)
                {
                    await _conductorService.ActualizarAsync(c, ct);
                }
            }

            // 🔥 actualizar extras
            if (request.Extras != null)
            {
                foreach (var e in request.Extras)
                {
                    if (e.Eliminar && e.Id.HasValue)
                        await _reservaExtraDataService.RemoveAsync(e.Id.Value, ct);
                    else if (e.Id.HasValue)
                        await _reservaExtraDataService.UpdateAsync(new ReservaExtraDataModel
                        {
                            Id = e.Id.Value,
                            IdExtra = e.IdExtra,
                            Cantidad = e.Cantidad,
                            FechaActualizacion = DateTime.UtcNow
                        }, ct);
                    else
                        await _reservaExtraDataService.AddAsync(new ReservaExtraDataModel
                        {
                            IdReserva = request.Id,
                            IdExtra = e.IdExtra,
                            Cantidad = e.Cantidad,
                            Estado = "ACT",
                            FechaCreacion = DateTime.UtcNow,
                            FechaActualizacion = DateTime.UtcNow
                        }, ct);
                }
            }

            return await ObtenerPorIdAsync(request.Id, ct);
        }

        // =========================
        // CONSULTAS
        // =========================
        public async Task<ReservaResponse> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            var model = await _reservaDataService.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Reserva", id);

            var response = ReservaBusinessMapper.ToResponse(model);

            var extras = await _reservaExtraDataService.GetByReservaAsync(id, ct);
            response.Extras = ReservaExtraBusinessMapper.ToResponseList(extras);

            var conductores = await _conductorService.ObtenerPorReservaAsync(id, ct);
            response.Conductores = conductores.ToList();

            return response;
        }

        public async Task<IReadOnlyList<ReservaResponse>> ListarAsync(CancellationToken ct = default)
        {
            var list = await _reservaDataService.GetAllAsync(ct);
            return ReservaBusinessMapper.ToResponseList(list);
        }

        public async Task<IReadOnlyList<ReservaResponse>> ObtenerPorClienteAsync(int idCliente, CancellationToken ct = default)
        {
            var list = await _reservaDataService.GetByClienteAsync(idCliente, ct);
            return ReservaBusinessMapper.ToResponseList(list);
        }

        public async Task<IReadOnlyList<ReservaResponse>> ObtenerPorVehiculoAsync(int idVehiculo, CancellationToken ct = default)
        {
            var list = await _reservaDataService.GetByVehiculoAsync(idVehiculo, ct);
            return ReservaBusinessMapper.ToResponseList(list);
        }

        public async Task<DataPagedResult<ReservaResponse>> BuscarAsync(ReservaFiltroRequest request, CancellationToken ct = default)
        {
            var data = await _reservaDataService.BuscarAsync(
                new ReservaFiltroDataModel
                {
                    IdCliente = request.IdCliente,
                    IdVehiculo = request.IdVehiculo,
                    Estado = request.Estado,
                    Page = request.Page,
                    PageSize = request.PageSize
                }, ct);

            return new DataPagedResult<ReservaResponse>(
                ReservaBusinessMapper.ToResponseList(data.Items),
                data.TotalRecords,
                data.Page,
                data.PageSize
            );
        }

        // =========================
        // DISPONIBILIDAD
        // =========================
        public async Task<bool> VerificarDisponibilidadVehiculoAsync(
            int idVehiculo,
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken ct = default)
        {
            return await _reservaDataService.IsVehiculoDisponibleAsync(
                idVehiculo, fechaInicio, fechaFin, ct);
        }

        // =========================
        // ACCIONES DE NEGOCIO
        // =========================
        public async Task<bool> ConfirmarAsync(int id, CancellationToken ct = default)
        {
            return await _reservaDataService.ConfirmarAsync(id, ct);
        }

        public async Task<bool> CancelarAsync(int id, string motivo, CancellationToken ct = default)
        {
            return await _reservaDataService.CancelarAsync(id, motivo, ct);
        }

        private async Task<(decimal subtotal, decimal iva, decimal total)> CalcularTotalesAsync(
            int idVehiculo,
            int cantidadDias,
            List<CrearReservaExtraDetalleRequest>? extrasRequest,
            CancellationToken ct)
        {
            // 🔹 1. Vehículo
            var vehiculo = await _vehiculoDataService.GetByIdAsync(idVehiculo, ct);
            if (vehiculo == null)
                throw new NotFoundException("Vehículo", idVehiculo);

            var subtotalVehiculo = vehiculo.PrecioBaseDia * cantidadDias;

            // 🔹 2. Extras
            decimal subtotalExtras = 0;

            if (extrasRequest != null && extrasRequest.Any())
            {
                foreach (var extraReq in extrasRequest)
                {
                    var extraDb = await _extraDataService.GetByIdAsync(extraReq.IdExtra, ct);
                    if (extraDb == null)
                        throw new NotFoundException("Extra", extraReq.IdExtra);

                    subtotalExtras += extraDb.ValorFijo * extraReq.Cantidad;
                }
            }

            // 🔹 3. Totales
            var subtotal = subtotalVehiculo + subtotalExtras;
            var iva = subtotal * 0.15m;
            var total = subtotal + iva;

            return (subtotal, iva, total);
        }

        private async Task ValidarReferenciasAsync(
            CrearReservaRequest request,
            CancellationToken ct)
        {
            var errors = new List<string>();

            var cliente = await _clienteDataService.GetByIdAsync(request.IdCliente, ct);
            if (cliente == null)
                errors.Add($"No existe el cliente con id {request.IdCliente}.");

            var vehiculo = await _vehiculoDataService.GetByIdAsync(request.IdVehiculo, ct);
            if (vehiculo == null)
                errors.Add($"No existe el vehículo con id {request.IdVehiculo}.");

            var locRecogida = await _localizacionDataService.GetByIdAsync(request.IdLocalizacionRecogida, ct);
            if (locRecogida == null)
                errors.Add($"No existe la localización de recogida con id {request.IdLocalizacionRecogida}.");

            var locEntrega = await _localizacionDataService.GetByIdAsync(request.IdLocalizacionEntrega, ct);
            if (locEntrega == null)
                errors.Add($"No existe la localización de entrega con id {request.IdLocalizacionEntrega}.");

            if (request.Extras != null && request.Extras.Any())
            {
                foreach (var extra in request.Extras)
                {
                    var extraDb = await _extraDataService.GetByIdAsync(extra.IdExtra, ct);
                    if (extraDb == null)
                        errors.Add($"No existe el extra con id {extra.IdExtra}.");
                }
            }

            if (request.Conductores != null && request.Conductores.Any())
            {
                foreach (var conductor in request.Conductores)
                {
                    var conductorDb = await _conductorDataService.GetByIdAsync(conductor.IdConductor, ct);
                    if (conductorDb == null)
                        errors.Add($"No existe el conductor con id {conductor.IdConductor}.");
                }
            }

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}
