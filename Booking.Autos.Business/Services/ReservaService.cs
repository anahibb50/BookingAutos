using Booking.Autos.Business.DTOs.Reserva;
using Booking.Autos.Business.DTOs.ReservaExtra;
using Booking.Autos.Business.DTOs.Conductor;
using Booking.Autos.Business.DTOs.ConductorReserva;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.Business.Validators;
using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Facturas;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.Business.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaDataService _reservaDataService;
        private readonly IReservaExtraDataService _reservaExtraDataService;
        private readonly IConductorReservaService _conductorReservaService;
        private readonly IConductorService _conductorService;
        private readonly IVehiculoDataService _vehiculoDataService;
        private readonly IExtraDataService _extraDataService;
        private readonly IClienteDataService _clienteDataService;
        private readonly ILocalizacionDataService _localizacionDataService;
        private readonly IConductorDataService _conductorDataService;
        private readonly IFacturaDataService _facturaDataService;

        public ReservaService(
            IReservaDataService reservaDataService,
            IReservaExtraDataService reservaExtraDataService,
            IConductorReservaService conductorReservaService,
            IConductorService conductorService,
            IExtraDataService extraDataService,
            IVehiculoDataService vehiculoDataService,
            IClienteDataService clienteDataService,
            ILocalizacionDataService localizacionDataService,
            IConductorDataService conductorDataService,
            IFacturaDataService facturaDataService)
        {
            _reservaDataService = reservaDataService;
            _reservaExtraDataService = reservaExtraDataService;
            _extraDataService = extraDataService;
            _conductorReservaService = conductorReservaService;
            _conductorService = conductorService;
            _vehiculoDataService = vehiculoDataService;
            _clienteDataService = clienteDataService;
            _localizacionDataService = localizacionDataService;
            _conductorDataService = conductorDataService;
            _facturaDataService = facturaDataService;
        }

        public async Task<ReservaResponse> CrearAsync(CrearReservaRequest request, CancellationToken ct = default)
        {
            var errors = ReservaValidator.ValidarCreacion(request);
            if (errors.Any())
                throw new ValidationException(errors.ToList());

            var cantidadDiasCalculada = CalcularCantidadDias(request.FechaInicio, request.HoraInicio, request.FechaFin, request.HoraFin);

            var vehiculo = await _vehiculoDataService.GetByIdAsync(request.IdVehiculo, ct);
            if (vehiculo == null)
                throw new ValidationException(new List<string> { $"No existe el vehículo con id {request.IdVehiculo}." });

            // La recogida siempre arranca desde la localización actual del vehículo.
            request.IdLocalizacionRecogida = vehiculo.IdLocalizacion;

            await ValidarReferenciasAsync(request, ct);

            var disponible = await _reservaDataService.IsVehiculoDisponibleAsync(
                request.IdVehiculo, request.FechaInicio, request.FechaFin, ct);

            if (!disponible)
                throw new ValidationException(new List<string> { "El vehículo no está disponible para el rango de fechas solicitado." });

            var model = ReservaBusinessMapper.ToDataModel(request);
            model.CantidadDias = cantidadDiasCalculada;
            model.CreadoPorUsuario = "SYSTEM";
            model.ServicioOrigen = "API";

            var (subtotal, iva, total) = await CalcularTotalesAsync(
                request.IdVehiculo,
                cantidadDiasCalculada,
                request.Extras,
                ct);

            model.Subtotal = subtotal;
            model.Iva = iva;
            model.Total = total;

            var reserva = await _reservaDataService.CreateAsync(model, ct);

            if (request.Extras != null)
            {
                foreach (var extra in request.Extras)
                {
                    await _reservaExtraDataService.AddAsync(new ReservaExtraDataModel
                    {
                        IdReserva = reserva.Id,
                        IdExtra = extra.IdExtra,
                        Cantidad = extra.Cantidad,
                        Estado = "PEN",
                        EsEliminado = false,
                        FechaCreacion = DateTime.UtcNow,
                        FechaActualizacion = DateTime.UtcNow
                    }, ct);
                }
            }

            if (request.Conductores != null)
            {
                foreach (var conductor in request.Conductores)
                {
                    var idConductor = conductor.IdConductor;

                    if (idConductor <= 0)
                    {
                        if (conductor.NuevoConductor == null)
                            throw new ValidationException(new List<string>
                            {
                                "Debes enviar IdConductor válido o el objeto NuevoConductor para crearlo."
                            });

                        var conductorCreado = await _conductorService.CrearAsync(conductor.NuevoConductor, ct);
                        idConductor = conductorCreado.Id;
                    }

                    await _conductorReservaService.CrearAsync(
                        reserva.Id,
                        new CrearConductorReservaDetalleRequest
                        {
                            IdConductor = idConductor,
                            Rol = conductor.Rol,
                            EsPrincipal = conductor.EsPrincipal,
                            Observaciones = conductor.Observaciones
                        },
                        ct);
                }
            }

            return await ObtenerPorIdAsync(reserva.Id, ct);
        }

        public async Task<ReservaResponse> ActualizarAsync(ActualizarReservaRequest request, CancellationToken ct = default)
        {
            var errors = ReservaValidator.ValidarActualizacion(request);
            if (errors.Any())
                throw new ValidationException(errors.ToList());

            var existente = await _reservaDataService.GetByIdAsync(request.Id, ct);
            if (existente == null)
                throw new NotFoundException("Reserva", request.Id);

            if (existente.Estado == "CON" || existente.Estado == "CAN")
                throw new ValidationException(new List<string> { "No se puede actualizar una reserva confirmada o cancelada." });

            var cantidadDiasCalculada = CalcularCantidadDias(request.FechaInicio, request.HoraInicio, request.FechaFin, request.HoraFin);

            var vehiculo = await _vehiculoDataService.GetByIdAsync(request.IdVehiculo, ct);
            if (vehiculo == null)
                throw new ValidationException(new List<string> { $"No existe el vehículo con id {request.IdVehiculo}." });

            // En actualización también se forza la localización de recogida desde el vehículo.
            request.IdLocalizacionRecogida = vehiculo.IdLocalizacion;

            await ValidarReferenciasActualizacionAsync(request, ct);
            await ValidarDisponibilidadActualizacionAsync(request.IdVehiculo, request.Id, request.FechaInicio, request.FechaFin, ct);

            var model = ReservaBusinessMapper.ToDataModel(request);
            model.CantidadDias = cantidadDiasCalculada;
            model.Codigo = existente.Codigo;
            model.Guid = existente.Guid;
            model.FechaReservaUtc = existente.FechaReservaUtc;
            model.Estado = existente.Estado;
            var (subtotal, iva, total) = await CalcularTotalesActualizacionAsync(
                request.IdVehiculo,
                cantidadDiasCalculada,
                request.Id,
                ct);
            model.Subtotal = subtotal;
            model.Iva = iva;
            model.Total = total;
            model.CreadoPorUsuario = existente.CreadoPorUsuario;
            model.ServicioOrigen = existente.ServicioOrigen;
            model.OrigenCanal = existente.OrigenCanal;
            model.EsEliminado = existente.EsEliminado;
            model.ModificadoPorUsuario = "SYSTEM";
            model.ModificacionIp = "127.0.0.1";

            await _reservaDataService.UpdateAsync(model, ct);

            return await ObtenerPorIdAsync(request.Id, ct);
        }

        public async Task<ReservaResponse> ObtenerPorIdAsync(int id, CancellationToken ct = default)
        {
            var model = await _reservaDataService.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Reserva", id);

            var response = ReservaBusinessMapper.ToResponse(model);

            var extras = await _reservaExtraDataService.GetByReservaAsync(id, ct);
            response.Extras = ReservaExtraBusinessMapper.ToResponseList(extras);

            var conductores = await _conductorReservaService.ObtenerPorReservaAsync(id, ct);
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
                    IdLocalizacionRecogida = request.IdLocalizacionRecogida,
                    IdLocalizacionEntrega = request.IdLocalizacionEntrega,
                    FechaInicioDesde = request.FechaInicioDesde,
                    FechaInicioHasta = request.FechaInicioHasta,
                    FechaFinDesde = request.FechaFinDesde,
                    FechaFinHasta = request.FechaFinHasta,
                    Estado = request.Estado,
                    CodigoReserva = request.CodigoReserva,
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

        public async Task<bool> VerificarDisponibilidadVehiculoAsync(
            int idVehiculo,
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken ct = default)
        {
            return await _reservaDataService.IsVehiculoDisponibleAsync(idVehiculo, fechaInicio, fechaFin, ct);
        }

        public async Task<bool> ConfirmarAsync(int id, CancellationToken ct = default)
        {
            var reserva = await _reservaDataService.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Reserva", id);

            var errors = ReservaValidator.ValidarConfirmacion(reserva.Estado);
            if (errors.Any())
                throw new ValidationException(errors.ToList());

            var confirmada = await _reservaDataService.ConfirmarAsync(id, ct);
            if (!confirmada)
                return false;

            var facturaExistente = await _facturaDataService.GetByReservaAsync(id, ct);

            if (facturaExistente == null)
            {
                var facturaCreada = await _facturaDataService.CreateAsync(new FacturaDataModel
                {
                    IdReserva = reserva.Id,
                    IdCliente = reserva.IdCliente,
                    Descripcion = $"Factura automática de la reserva {reserva.Codigo}",
                    Origen = "RESERVA_CONFIRMADA",
                    Subtotal = reserva.Subtotal,
                    Iva = reserva.Iva,
                    Total = reserva.Total,
                    Estado = "ABI",
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow,
                    EsEliminado = false
                }, ct);

                await _facturaDataService.AprobarAsync(facturaCreada.Id, ct);
            }
            else if (facturaExistente.Estado != "APR")
            {
                await _facturaDataService.AprobarAsync(facturaExistente.Id, ct);
            }

            return true;
        }

        public async Task<bool> CancelarAsync(int id, string motivo, CancellationToken ct = default)
        {
            var reserva = await _reservaDataService.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Reserva", id);

            var errors = ReservaValidator.ValidarCancelacion(reserva.Estado, motivo);
            if (errors.Any())
                throw new ValidationException(errors.ToList());

            return await _reservaDataService.CancelarAsync(id, motivo, ct);
        }

        private async Task<(decimal subtotal, decimal iva, decimal total)> CalcularTotalesAsync(
            int idVehiculo,
            int cantidadDias,
            List<CrearReservaExtraDetalleRequest>? extrasRequest,
            CancellationToken ct)
        {
            var vehiculo = await _vehiculoDataService.GetByIdAsync(idVehiculo, ct);
            if (vehiculo == null)
                throw new NotFoundException("Vehículo", idVehiculo);

            var subtotalVehiculo = vehiculo.PrecioBaseDia * cantidadDias;
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

            var subtotal = subtotalVehiculo + subtotalExtras;
            var iva = subtotal * 0.15m;
            var total = subtotal + iva;

            return (subtotal, iva, total);
        }

        private async Task ValidarReferenciasAsync(CrearReservaRequest request, CancellationToken ct)
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
                var extrasDuplicados = request.Extras
                    .GroupBy(x => x.IdExtra)
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Key)
                    .ToList();

                if (extrasDuplicados.Any())
                    errors.Add($"No se permiten extras duplicados en la reserva: {string.Join(", ", extrasDuplicados)}.");

                foreach (var extra in request.Extras)
                {
                    if (extra.Cantidad <= 0)
                    {
                        errors.Add($"La cantidad del extra {extra.IdExtra} debe ser mayor a 0.");
                        continue;
                    }

                    var extraDb = await _extraDataService.GetByIdAsync(extra.IdExtra, ct);
                    if (extraDb == null)
                        errors.Add($"No existe el extra con id {extra.IdExtra}.");
                }
            }

            if (request.Conductores != null && request.Conductores.Any())
            {
                var conductoresDuplicados = request.Conductores
                    .GroupBy(x => x.IdConductor)
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Key)
                    .ToList();

                if (conductoresDuplicados.Any())
                    errors.Add($"No se permiten conductores duplicados en la reserva: {string.Join(", ", conductoresDuplicados)}.");

                foreach (var conductor in request.Conductores)
                {
                    if (string.IsNullOrWhiteSpace(conductor.Rol))
                    {
                        errors.Add($"El rol del conductor {conductor.IdConductor} es obligatorio.");
                        continue;
                    }

                    if (conductor.IdConductor > 0)
                    {
                        var conductorDb = await _conductorDataService.GetByIdAsync(conductor.IdConductor, ct);
                        if (conductorDb == null)
                            errors.Add($"No existe el conductor con id {conductor.IdConductor}.");
                    }
                    else if (conductor.NuevoConductor == null)
                    {
                        errors.Add("Debes enviar IdConductor válido o el objeto NuevoConductor para crearlo.");
                    }
                }
            }

            if (errors.Any())
                throw new ValidationException(errors);
        }

        private async Task ValidarReferenciasActualizacionAsync(ActualizarReservaRequest request, CancellationToken ct)
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

            if (errors.Any())
                throw new ValidationException(errors);
        }

        private async Task ValidarDisponibilidadActualizacionAsync(
            int idVehiculo,
            int idReservaActual,
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken ct)
        {
            var reservasVehiculo = await _reservaDataService.GetByVehiculoAsync(idVehiculo, ct);

            var conflicto = reservasVehiculo.Any(r =>
                r.Id != idReservaActual &&
                (r.Estado == "PEN" || r.Estado == "CON") &&
                fechaInicio < r.FechaFin &&
                fechaFin > r.FechaInicio);

            if (conflicto)
                throw new ValidationException(new List<string> { "El vehículo no está disponible para el nuevo rango de fechas solicitado." });
        }

        private async Task<(decimal subtotal, decimal iva, decimal total)> CalcularTotalesActualizacionAsync(
            int idVehiculo,
            int cantidadDias,
            int idReserva,
            CancellationToken ct)
        {
            var vehiculo = await _vehiculoDataService.GetByIdAsync(idVehiculo, ct);
            if (vehiculo == null)
                throw new NotFoundException("Vehículo", idVehiculo);

            var subtotalVehiculo = vehiculo.PrecioBaseDia * cantidadDias;
            var subtotalExtras = await _reservaExtraDataService.GetSubtotalByReservaAsync(idReserva, ct);
            var subtotal = subtotalVehiculo + subtotalExtras;
            var iva = subtotal * 0.15m;
            var total = subtotal + iva;

            return (subtotal, iva, total);
        }

        private static int CalcularCantidadDias(DateTime fechaInicio, TimeSpan? horaInicio, DateTime fechaFin, TimeSpan? horaFin)
        {
            var inicio = fechaInicio.Date.Add(horaInicio ?? fechaInicio.TimeOfDay);
            var fin = fechaFin.Date.Add(horaFin ?? fechaFin.TimeOfDay);
            var duracion = fin - inicio;

            // Ya se valida previamente que inicio < fin, aquí solo dejamos un mínimo defensivo.
            if (duracion.TotalHours <= 0)
                return 1;

            return (int)Math.Ceiling(duracion.TotalHours / 24d);
        }
    }
}
