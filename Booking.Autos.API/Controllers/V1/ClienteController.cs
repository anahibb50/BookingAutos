using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.DTOs.Cliente;
using Booking.Autos.API.Models.Common;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/clientes")]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // ============================================================
        // 📌 CREAR
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearClienteRequest request,
            CancellationToken ct)
        {
            var result = await _clienteService.CrearAsync(request, ct);

            return Ok(ApiResponse<ClienteResponse>.Ok(result, "Cliente creado"));
        }

        // ============================================================
        // ✏️ ACTUALIZAR
        // ============================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarClienteRequest request,
            CancellationToken ct)
        {
            request.Id = id;

            var result = await _clienteService.ActualizarAsync(request, ct);

            return Ok(ApiResponse<ClienteResponse>.Ok(result, "Cliente actualizado"));
        }

        // ============================================================
        // ❌ ELIMINAR LÓGICO
        // ============================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(
            int id,
            CancellationToken ct)
        {
            var usuario = User?.Identity?.Name ?? "system";

            await _clienteService.EliminarLogicoAsync(id, usuario, ct);

            return Ok(ApiResponse<string>.Ok("OK", "Cliente eliminado"));
        }

        // ============================================================
        // 🔍 OBTENER POR ID
        // ============================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(
            int id,
            CancellationToken ct)
        {
            var result = await _clienteService.ObtenerPorIdAsync(id, ct);

            return Ok(ApiResponse<ClienteResponse>.Ok(result));
        }

        // ============================================================
        // 🔍 POR IDENTIFICACIÓN (🔥 CLAVE)
        // ============================================================
        [HttpGet("por-identificacion/{identificacion}")]
        public async Task<IActionResult> ObtenerPorIdentificacion(
            string identificacion,
            CancellationToken ct)
        {
            var result = await _clienteService.ObtenerPorIdentificacionAsync(identificacion, ct);

            return Ok(ApiResponse<ClienteResponse?>.Ok(result));
        }

        // ============================================================
        // 📄 LISTAR
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _clienteService.ListarAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<ClienteResponse>>.Ok(result));
        }

        // ============================================================
        // 🔍 BÚSQUEDA PAGINADA 🔥🔥🔥
        // ============================================================
        [HttpPost("buscar")]
        public async Task<IActionResult> Buscar(
            [FromBody] ClienteFiltroRequest request,
            CancellationToken ct)
        {
            var result = await _clienteService.BuscarAsync(request, ct);

            return Ok(ApiResponse<DataPagedResult<ClienteResponse>>.Ok(result));
        }

        // ============================================================
        // 🔍 VALIDACIÓN
        // ============================================================
        [HttpGet("existe")]
        public async Task<IActionResult> Existe(
            [FromQuery] string identificacion,
            CancellationToken ct)
        {
            var existe = await _clienteService.ExistePorIdentificacionAsync(identificacion, ct);

            return Ok(ApiResponse<bool>.Ok(existe));
        }
    }
}