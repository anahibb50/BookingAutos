using Booking.Autos.Business.DTOs.Reserva;
using Booking.Autos.Business.DTOs.ReservaExtra;
using Booking.Autos.Business.DTOs.ConductorReserva;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.Mappers;
using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.Business.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaDataService _reservaDataService;
        private readonly IReservaExtraDataService _extraDataService;
        private readonly IConductorReservaService _conductorService;

        public ReservaService(
            IReservaDataService reservaDataService,
            IReservaExtraDataService extraDataService,
            IConductorReservaService conductorService)
        {
            _reservaDataService = reservaDataService;
            _extraDataService = extraDataService;
            _conductorService = conductorService;
        }

        // =========================
        // CREAR
        // =========================
        public async Task<ReservaResponse> CrearAsync(
            CrearReservaRequest request,
            CancellationToken ct = default)
        {
            if (request.IdCliente <= 0 || request.IdVehiculo <= 0)
                throw new ValidationException(new List<string> { "Datos inválidos." });

            var disponible = await _reservaDataService.IsVehiculoDisponibleAsync(
                request.IdVehiculo, request.FechaInicio, request.FechaFin, ct);

            if (!disponible)
                throw new BusinessException("Vehículo no disponible.");

            var model = ReservaBusinessMapper.ToDataModel(request);
            model.CreadoPorUsuario = "SYSTEM";
            model.ServicioOrigen = "API";

            var reserva = await _reservaDataService.CreateAsync(model, ct);

            // 🔥 EXTRAS
            if (request.Extras != null)
            {
                foreach (var extra in request.Extras)
                {
                    await _extraDataService.AddAsync(new ReservaExtraDataModel
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
                        await _extraDataService.RemoveAsync(e.Id.Value, ct);
                    else if (e.Id.HasValue)
                        await _extraDataService.UpdateAsync(new ReservaExtraDataModel
                        {
                            Id = e.Id.Value,
                            IdExtra = e.IdExtra,
                            Cantidad = e.Cantidad,
                            FechaActualizacion = DateTime.UtcNow
                        }, ct);
                    else
                        await _extraDataService.AddAsync(new ReservaExtraDataModel
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

            var extras = await _extraDataService.GetByReservaAsync(id, ct);
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
    }
}