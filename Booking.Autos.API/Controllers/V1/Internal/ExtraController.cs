using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.DTOs.Extra;
using Booking.Autos.API.Models.Common;

namespace Booking.Autos.API.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/extras")]
    [Authorize]
    public class ExtraController : ControllerBase
    {
        private readonly IExtraService _extraService;

        public ExtraController(IExtraService extraService)
        {
            _extraService = extraService;
        }

        // ============================================================
        // 📌 CREAR
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearExtraRequest request,
            CancellationToken ct)
        {
            var result = await _extraService.CrearAsync(request, ct);

            return Ok(ApiResponse<ExtraResponse>.Ok(result, "Extra creado"));
        }

        // ============================================================
        // ✏️ ACTUALIZAR
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarExtraRequest request,
            CancellationToken ct)
        {
            request.Id = id;

            var result = await _extraService.ActualizarAsync(request, ct);

            return Ok(ApiResponse<ExtraResponse>.Ok(result, "Extra actualizado"));
        }

        // ============================================================
        // ❌ ELIMINAR LÓGICO
        // ============================================================
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(
            int id,
            CancellationToken ct)
        {
            var usuario = User?.Identity?.Name ?? "system";

            await _extraService.EliminarLogicoAsync(id, usuario, ct);

            return Ok(ApiResponse<string>.Ok("OK", "Extra eliminado"));
        }

        // ============================================================
        // 🔍 OBTENER POR ID
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR,CLIENTE")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(
            int id,
            CancellationToken ct)
        {
            var result = await _extraService.ObtenerPorIdAsync(id, ct);

            return Ok(ApiResponse<ExtraResponse>.Ok(result));
        }

        // ============================================================
        // 📄 LISTAR
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR,CLIENTE")]
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _extraService.ListarAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<ExtraResponse>>.Ok(result));
        }

        // ============================================================
        // 🔥 SOLO ACTIVOS
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR,CLIENTE")]
        [HttpGet("activos")]
        public async Task<IActionResult> ListarActivos(CancellationToken ct)
        {
            var result = await _extraService.ListarActivosAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<ExtraResponse>>.Ok(result));
        }

        // ============================================================
        // 🔍 BUSCAR POR NOMBRE
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR,CLIENTE")]
        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarPorNombre(
            [FromQuery] string nombre,
            CancellationToken ct)
        {
            var result = await _extraService.ObtenerPorNombreAsync(nombre, ct);

            return Ok(ApiResponse<IReadOnlyList<ExtraResponse>>.Ok(result));
        }

        // ============================================================
        // 💰 ACTUALIZAR PRECIO (🔥 ESPECIAL)
        // ============================================================
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("{id}/precio")]
        public async Task<IActionResult> ActualizarPrecio(
            int id,
            [FromQuery] decimal nuevoPrecio,
            CancellationToken ct)
        {
            var actualizado = await _extraService.ActualizarPrecioAsync(id, nuevoPrecio, ct);

            return Ok(ApiResponse<bool>.Ok(actualizado, "Precio actualizado"));
        }
    }
}   
