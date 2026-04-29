using Asp.Versioning;
using Booking.Autos.API.Models.Common;
using Booking.Autos.Business.DTOs.Reserva;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.DataManagement.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Autos.API.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/reservas")]
    [Authorize]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [Authorize(Roles = "ADMIN,VENDEDOR,CLIENTE")]
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearReservaRequest? request, CancellationToken ct)
        {
            if (request == null)
                return BadRequest(new ApiErrorResponse("El cuerpo de la solicitud es obligatorio."));

            var result = await _reservaService.CrearAsync(request, ct);
            return Ok(ApiResponse<ReservaResponse>.Ok(result, "Reserva creada"));
        }

        [Authorize(Roles = "CLIENTE")]
        [HttpPost("crear-y-confirmar")]
        public async Task<IActionResult> CrearYConfirmar([FromBody] CrearReservaRequest? request, CancellationToken ct)
        {
            if (request == null)
                return BadRequest(new ApiErrorResponse("El cuerpo de la solicitud es obligatorio."));

            var idClienteSesion = ObtenerIdClienteDesdeToken();
            if (!idClienteSesion.HasValue)
                return StatusCode(StatusCodes.Status403Forbidden, new ApiErrorResponse("No se pudo determinar el cliente de la sesión."));

            if (request.IdCliente != idClienteSesion.Value)
                return StatusCode(StatusCodes.Status403Forbidden, new ApiErrorResponse("No puedes crear reservas para otro cliente."));

            var creada = await _reservaService.CrearAsync(request, ct);
            var confirmada = await _reservaService.ConfirmarAsync(creada.Id, ct);

            if (!confirmada)
                return StatusCode(StatusCodes.Status409Conflict, new ApiErrorResponse("No se pudo confirmar la reserva recién creada."));

            var result = await _reservaService.ObtenerPorIdAsync(creada.Id, ct);
            return Ok(ApiResponse<ReservaResponse>.Ok(result, "Reserva creada y confirmada"));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarReservaRequest? request, CancellationToken ct)
        {
            if (request == null)
                return BadRequest(new ApiErrorResponse("El cuerpo de la solicitud es obligatorio."));

            request.Id = id;

            var result = await _reservaService.ActualizarAsync(request, ct);
            return Ok(ApiResponse<ReservaResponse>.Ok(result, "Reserva actualizada"));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR,CLIENTE")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
        {
            if (User.IsInRole("CLIENTE"))
            {
                var idClienteSesion = ObtenerIdClienteDesdeToken();
                if (!idClienteSesion.HasValue)
                    return StatusCode(StatusCodes.Status403Forbidden, new ApiErrorResponse("No se pudo determinar el cliente de la sesión."));

                var reservaCliente = await _reservaService.ObtenerPorIdAsync(id, ct);
                if (reservaCliente.IdCliente != idClienteSesion.Value)
                    return StatusCode(StatusCodes.Status403Forbidden, new ApiErrorResponse("No puedes ver reservas de otro cliente."));

                return Ok(ApiResponse<ReservaResponse>.Ok(reservaCliente));
            }

            var result = await _reservaService.ObtenerPorIdAsync(id, ct);
            return Ok(ApiResponse<ReservaResponse>.Ok(result));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _reservaService.ListarAsync(ct);
            return Ok(ApiResponse<IReadOnlyList<ReservaResponse>>.Ok(result));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet("por-cliente/{idCliente}")]
        public async Task<IActionResult> PorCliente(int idCliente, CancellationToken ct)
        {
            var result = await _reservaService.ObtenerPorClienteAsync(idCliente, ct);
            return Ok(ApiResponse<IReadOnlyList<ReservaResponse>>.Ok(result));
        }

        [Authorize(Roles = "CLIENTE")]
        [HttpGet("mis-reservas")]
        public async Task<IActionResult> MisReservas(CancellationToken ct)
        {
            var idClienteSesion = ObtenerIdClienteDesdeToken();
            if (!idClienteSesion.HasValue)
                return StatusCode(StatusCodes.Status403Forbidden, new ApiErrorResponse("No se pudo determinar el cliente de la sesión."));

            var result = await _reservaService.ObtenerPorClienteAsync(idClienteSesion.Value, ct);
            return Ok(ApiResponse<IReadOnlyList<ReservaResponse>>.Ok(result));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet("por-vehiculo/{idVehiculo}")]
        public async Task<IActionResult> PorVehiculo(int idVehiculo, CancellationToken ct)
        {
            var result = await _reservaService.ObtenerPorVehiculoAsync(idVehiculo, ct);
            return Ok(ApiResponse<IReadOnlyList<ReservaResponse>>.Ok(result));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPost("buscar")]
        public async Task<IActionResult> Buscar([FromBody] ReservaFiltroRequest request, CancellationToken ct)
        {
            var result = await _reservaService.BuscarAsync(request, ct);
            return Ok(ApiResponse<DataPagedResult<ReservaResponse>>.Ok(result));
        }

        [AllowAnonymous]
        [HttpGet("disponibilidad")]
        public async Task<IActionResult> VerificarDisponibilidad(
            [FromQuery] int idVehiculo,
            [FromQuery] DateTime fechaInicio,
            [FromQuery] DateTime fechaFin,
            CancellationToken ct)
        {
            var disponible = await _reservaService.VerificarDisponibilidadVehiculoAsync(idVehiculo, fechaInicio, fechaFin, ct);
            return Ok(ApiResponse<bool>.Ok(disponible));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPost("{id}/confirmar")]
        public async Task<IActionResult> Confirmar(int id, CancellationToken ct)
        {
            var ok = await _reservaService.ConfirmarAsync(id, ct);
            return Ok(ApiResponse<bool>.Ok(ok, "Reserva confirmada"));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPost("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id, [FromQuery] string motivo, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                return BadRequest(new ApiErrorResponse("El motivo es obligatorio"));

            var ok = await _reservaService.CancelarAsync(id, motivo, ct);
            return Ok(ApiResponse<bool>.Ok(ok, "Reserva cancelada"));
        }

        private int? ObtenerIdClienteDesdeToken()
        {
            var claim = User.FindFirst("id_cliente")?.Value;
            if (int.TryParse(claim, out var idCliente))
                return idCliente;

            return null;
        }
    }
}
