using Asp.Versioning;
using Booking.Autos.API.Models.Booking;
using Booking.Autos.API.Models.Common;
using Booking.Autos.Business.DTOs.Vehiculo;
using Booking.Autos.Business.Exceptions;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.DataManagement.Common;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Autos.API.Controllers.V1.Booking
{
    /// <summary>
    /// Endpoints públicos de vehículos para integración con Booking / OTA.
    /// vehiculoId corresponde a CodigoInterno.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/booking/vehiculos")]
    public class BookingVehiculosController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;
        private readonly IReservaService _reservaService;

        public BookingVehiculosController(
            IVehiculoService vehiculoService,
            IReservaService reservaService)
        {
            _vehiculoService = vehiculoService;
            _reservaService = reservaService;
        }

        /// <summary>
        /// Endpoint 1: Búsqueda paginada de vehículos disponibles.
        /// GET /api/v1/vehiculos?idLocalizacion=1&fechaRecogida=...&fechaDevolucion=...
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> BuscarVehiculos(
            [FromQuery] BookingBuscarVehiculosRequest request,
            CancellationToken ct)
        {
            var filtro = new VehiculoFiltroRequest
            {
                IdLocalizacion = request.IdLocalizacion,
                FechaInicio = request.FechaRecogida,
                FechaFin = request.FechaDevolucion,
                Page = request.Page <= 0 ? 1 : request.Page,
                PageSize = request.Limit <= 0 ? 20 : request.Limit
            };

            var result = await _vehiculoService.BuscarAsync(filtro, ct);
            return Ok(ApiResponse<DataPagedResult<VehiculoResponse>>.Ok(result));
        }

        /// <summary>
        /// Endpoint 2: Detalle completo de un vehículo específico.
        /// GET /api/v1/vehiculos/{vehiculoId}
        /// </summary>
        [HttpGet("{vehiculoId}")]
        public async Task<IActionResult> GetDetalle(string vehiculoId, CancellationToken ct)
        {
            var vehiculo = await ObtenerVehiculoAsync(vehiculoId, ct);
            if (vehiculo is null)
                return NotFound(new ApiErrorResponse($"No existe un vehículo con id o código {vehiculoId}."));

            return Ok(ApiResponse<VehiculoResponse>.Ok(vehiculo));
        }

        /// <summary>
        /// Endpoint 3: Verificar disponibilidad en tiempo real de un vehículo.
        /// GET /api/v1/vehiculos/{vehiculoId}/disponibilidad?fechaRecogida=...&fechaDevolucion=...&idLocalizacion=...
        /// </summary>
        [HttpGet("{vehiculoId}/disponibilidad")]
        public async Task<IActionResult> VerificarDisponibilidad(
            string vehiculoId,
            [FromQuery] BookingDisponibilidadRequest request,
            CancellationToken ct)
        {
            var vehiculo = await ObtenerVehiculoAsync(vehiculoId, ct);
            if (vehiculo is null)
                return NotFound(new ApiErrorResponse($"No existe un vehículo con id o código {vehiculoId}."));

            if (request.IdLocalizacion.HasValue && vehiculo.IdLocalizacion != request.IdLocalizacion.Value)
            {
                return Ok(ApiResponse<bool>.Ok(false));
            }

            var disponible = await _reservaService.VerificarDisponibilidadVehiculoAsync(
                vehiculo.Id,
                request.FechaRecogida,
                request.FechaDevolucion,
                ct);

            return Ok(ApiResponse<bool>.Ok(disponible));
        }

        private async Task<VehiculoResponse?> ObtenerVehiculoAsync(
            string vehiculoId,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(vehiculoId))
                return null;

            if (int.TryParse(vehiculoId, out var idVehiculo))
            {
                try
                {
                    return await _vehiculoService.ObtenerPorIdAsync(idVehiculo, ct);
                }
                catch (NotFoundException)
                {
                    return null;
                }
            }

            var list = await _vehiculoService.ListarAsync(ct);
            return list.FirstOrDefault(x =>
                string.Equals(x.CodigoInterno, vehiculoId.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
