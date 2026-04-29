using System.Security.Claims;
using Asp.Versioning;
using Booking.Autos.API.Models.Booking;
using Booking.Autos.API.Models.Common;
using Booking.Autos.Business.DTOs.Factura;
using Booking.Autos.Business.DTOs.Reserva;
using Booking.Autos.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Autos.API.Controllers.V1.Booking
{
    /// <summary>
    /// Endpoints públicos de reservas para integración Booking / OTA.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/booking/reservas")]
    public class BookingReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly IFacturaService _facturaService;

        public BookingReservasController(
            IReservaService reservaService,
            IFacturaService facturaService)
        {
            _reservaService = reservaService;
            _facturaService = facturaService;
        }

        /// <summary>
        /// Endpoint 8: Crear una nueva reserva.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearReservaRequest request,
            CancellationToken ct)
        {
            var result = await _reservaService.CrearAsync(request, ct);
            return Ok(ApiResponse<ReservaResponse>.Ok(result, "Reserva creada"));
        }

        /// <summary>
        /// Endpoint 9: Obtener detalle de una reserva por su código.
        /// </summary>
        [HttpGet("{codigoReserva}")]
        public async Task<IActionResult> GetByCodigo(
            string codigoReserva,
            CancellationToken ct)
        {
            var reserva = await ObtenerReservaPorCodigoAsync(codigoReserva, ct);
            if (reserva is null)
                return NotFound(new ApiErrorResponse($"No existe una reserva con el código {codigoReserva}."));

            return Ok(ApiResponse<ReservaResponse>.Ok(reserva));
        }

        /// <summary>
        /// Endpoint 10: Cancelar una reserva.
        /// </summary>
        [HttpPatch("{codigoReserva}/cancelar")]
        public async Task<IActionResult> Cancelar(
            string codigoReserva,
            [FromBody] BookingCancelarReservaRequest request,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.Motivo))
                return BadRequest(new ApiErrorResponse("El motivo es obligatorio."));

            var reserva = await ObtenerReservaPorCodigoAsync(codigoReserva, ct);
            if (reserva is null)
                return NotFound(new ApiErrorResponse($"No existe una reserva con el código {codigoReserva}."));

            var usuario = User?.FindFirstValue(ClaimTypes.Name) ?? "BOOKING";
            var motivo = request.Motivo.Trim();
            var ok = await _reservaService.CancelarAsync(reserva.Id, $"{motivo} | Usuario: {usuario}", ct);

            return Ok(ApiResponse<bool>.Ok(ok, "Reserva cancelada"));
        }

        /// <summary>
        /// Endpoint 11: Obtener la factura asociada a la reserva.
        /// </summary>
        [HttpGet("{codigoReserva}/factura")]
        public async Task<IActionResult> GetFactura(
            string codigoReserva,
            CancellationToken ct)
        {
            var reserva = await ObtenerReservaPorCodigoAsync(codigoReserva, ct);
            if (reserva is null)
                return NotFound(new ApiErrorResponse($"No existe una reserva con el código {codigoReserva}."));

            var factura = await _facturaService.ObtenerPorReservaAsync(reserva.Id, ct);
            if (factura is null)
                return NotFound(new ApiErrorResponse($"La reserva {codigoReserva} no tiene factura asociada."));

            return Ok(ApiResponse<FacturaResponse>.Ok(factura));
        }

        /// <summary>
        /// Verificar disponibilidad de vehículo por rango de fechas.
        /// </summary>
        [HttpGet("disponibilidad")]
        public async Task<IActionResult> VerificarDisponibilidad(
            [FromQuery] int idVehiculo,
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin,
            CancellationToken ct)
        {
            var disponible = await _reservaService
                .VerificarDisponibilidadVehiculoAsync(idVehiculo, fechaInicio, fechaFin, ct);

            return Ok(ApiResponse<bool>.Ok(disponible));
        }

        private async Task<ReservaResponse?> ObtenerReservaPorCodigoAsync(
            string codigoReserva,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(codigoReserva))
                return null;

            var resultado = await _reservaService.BuscarAsync(
                new ReservaFiltroRequest
                {
                    CodigoReserva = codigoReserva.Trim(),
                    Page = 1,
                    PageSize = 1
                },
                ct);

            return resultado.Items.FirstOrDefault();
        }
    }
}
