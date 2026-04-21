using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.DTOs.Factura;
using Booking.Autos.API.Models.Common;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/facturas")]
    [Authorize]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturaController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        // ============================================================
        // 📌 CREAR
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearFacturaRequest request,
            CancellationToken ct)
        {
            var result = await _facturaService.CrearAsync(request, ct);

            return Ok(ApiResponse<FacturaResponse>.Ok(result, "Factura creada"));
        }

        // ============================================================
        // 🔍 OBTENER POR ID
        // ============================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(
            int id,
            CancellationToken ct)
        {
            var result = await _facturaService.ObtenerPorIdAsync(id, ct);

            return Ok(ApiResponse<FacturaResponse>.Ok(result));
        }

        // ============================================================
        // 📄 LISTAR
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _facturaService.ListarAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<FacturaResponse>>.Ok(result));
        }

        // ============================================================
        // 🔥 POR CLIENTE
        // ============================================================
        [HttpGet("por-cliente/{idCliente}")]
        public async Task<IActionResult> ObtenerPorCliente(
            int idCliente,
            CancellationToken ct)
        {
            var result = await _facturaService.ObtenerPorClienteAsync(idCliente, ct);

            return Ok(ApiResponse<IReadOnlyList<FacturaResponse>>.Ok(result));
        }

        // ============================================================
        // 🔥 POR RESERVA (CLAVE)
        // ============================================================
        [HttpGet("por-reserva/{idReserva}")]
        public async Task<IActionResult> ObtenerPorReserva(
            int idReserva,
            CancellationToken ct)
        {
            var result = await _facturaService.ObtenerPorReservaAsync(idReserva, ct);

            return Ok(ApiResponse<FacturaResponse?>.Ok(result));
        }

        // ============================================================
        // 🔍 BÚSQUEDA PAGINADA 🔥🔥🔥
        // ============================================================
        [HttpPost("buscar")]
        public async Task<IActionResult> Buscar(
            [FromBody] FacturaFiltroRequest request,
            CancellationToken ct)
        {
            var result = await _facturaService.BuscarAsync(request, ct);

            return Ok(ApiResponse<DataPagedResult<FacturaResponse>>.Ok(result));
        }

        // ============================================================
        // ✅ APROBAR FACTURA (🔥 NEGOCIO)
        // ============================================================
        [HttpPost("{id}/aprobar")]
        public async Task<IActionResult> Aprobar(
            int id,
            CancellationToken ct)
        {
            var aprobado = await _facturaService.AprobarAsync(id, ct);

            return Ok(ApiResponse<bool>.Ok(aprobado, "Factura aprobada"));
        }

        // ============================================================
        // ❌ ANULAR FACTURA (🔥 NEGOCIO)
        // ============================================================
        [HttpPost("{id}/anular")]
        public async Task<IActionResult> Anular(
            int id,
            [FromQuery] string motivo,
            CancellationToken ct)
        {
            var anulada = await _facturaService.AnularAsync(id, motivo, ct);

            return Ok(ApiResponse<bool>.Ok(anulada, "Factura anulada"));
        }
    }
}