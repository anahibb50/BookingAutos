using Asp.Versioning;
using Booking.Autos.API.Models.Common;
using Booking.Autos.Business.DTOs.Factura;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.DataManagement.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Autos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/facturas")]
    [Authorize]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturaController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearFacturaRequest request, CancellationToken ct)
        {
            var result = await _facturaService.CrearAsync(request, ct);
            return Ok(ApiResponse<FacturaResponse>.Ok(result, "Factura creada"));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarFacturaRequest request, CancellationToken ct)
        {
            request.Id = id;

            var result = await _facturaService.ActualizarAsync(request, ct);
            return Ok(ApiResponse<FacturaResponse>.Ok(result, "Factura actualizada"));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
        {
            var result = await _facturaService.ObtenerPorIdAsync(id, ct);
            return Ok(ApiResponse<FacturaResponse>.Ok(result));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _facturaService.ListarAsync(ct);
            return Ok(ApiResponse<IReadOnlyList<FacturaResponse>>.Ok(result));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet("por-cliente/{idCliente}")]
        public async Task<IActionResult> ObtenerPorCliente(int idCliente, CancellationToken ct)
        {
            var result = await _facturaService.ObtenerPorClienteAsync(idCliente, ct);
            return Ok(ApiResponse<IReadOnlyList<FacturaResponse>>.Ok(result));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet("por-reserva/{idReserva}")]
        public async Task<IActionResult> ObtenerPorReserva(int idReserva, CancellationToken ct)
        {
            var result = await _facturaService.ObtenerPorReservaAsync(idReserva, ct);
            return Ok(ApiResponse<FacturaResponse?>.Ok(result));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPost("buscar")]
        public async Task<IActionResult> Buscar([FromBody] FacturaFiltroRequest request, CancellationToken ct)
        {
            var result = await _facturaService.BuscarAsync(request, ct);
            return Ok(ApiResponse<DataPagedResult<FacturaResponse>>.Ok(result));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPost("{id}/aprobar")]
        public async Task<IActionResult> Aprobar(int id, CancellationToken ct)
        {
            var aprobado = await _facturaService.AprobarAsync(id, ct);
            return Ok(ApiResponse<bool>.Ok(aprobado, "Factura aprobada"));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPost("{id}/anular")]
        public async Task<IActionResult> Anular(int id, [FromQuery] string motivo, CancellationToken ct)
        {
            var anulada = await _facturaService.AnularAsync(id, motivo, ct);
            return Ok(ApiResponse<bool>.Ok(anulada, "Factura anulada"));
        }
    }
}
