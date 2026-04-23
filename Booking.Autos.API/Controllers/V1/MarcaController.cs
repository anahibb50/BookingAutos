using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.DTOs.Catalogos.Marca;
using Booking.Autos.Business.DTOs.Marca;
using Booking.Autos.API.Models.Common;

namespace Booking.Autos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/marcas")]
    [Authorize]
    public class MarcaController : ControllerBase
    {
        private readonly IMarcaService _marcaService;

        public MarcaController(IMarcaService marcaService)
        {
            _marcaService = marcaService;
        }

        // ============================================================
        // 📌 CREAR
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearMarcaRequest request,
            CancellationToken ct)
        {
            if (request == null)
                return BadRequest(new ApiErrorResponse("Request inválido"));

            var result = await _marcaService.CrearAsync(request, ct);

            return Ok(ApiResponse<MarcaResponse>.Ok(result, "Marca creada"));
        }

        // ============================================================
        // ✏️ ACTUALIZAR
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarMarcaRequest request,
            CancellationToken ct)
        {
            if (request == null)
                return BadRequest(new ApiErrorResponse("Request inválido"));

            request.Id = id;

            var result = await _marcaService.ActualizarAsync(request, ct);

            return Ok(ApiResponse<MarcaResponse>.Ok(result, "Marca actualizada"));
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
            var usuario = User?.Identity?.Name ?? "anonymous";

            await _marcaService.EliminarLogicoAsync(id, usuario, ct);

            return Ok(ApiResponse<string>.Ok("OK", "Marca eliminada"));
        }

        // ============================================================
        // 🔍 OBTENER POR ID
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(
            int id,
            CancellationToken ct)
        {
            var result = await _marcaService.ObtenerPorIdAsync(id, ct);

            return Ok(ApiResponse<MarcaResponse>.Ok(result));
        }

        // ============================================================
        // 📄 LISTAR
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _marcaService.ListarAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<MarcaResponse>>.Ok(result));
        }

        // ============================================================
        // 🔍 POR NOMBRE
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet("por-nombre")]
        public async Task<IActionResult> ObtenerPorNombre(
            [FromQuery] string nombre,
            CancellationToken ct)
        {
            var result = await _marcaService.ObtenerPorNombreAsync(nombre, ct);

            return Ok(ApiResponse<MarcaResponse?>.Ok(result));
        }

        // ============================================================
        // 🔍 VALIDACIÓN
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet("existe")]
        public async Task<IActionResult> Existe(
            [FromQuery] string nombre,
            CancellationToken ct)
        {
            var existe = await _marcaService.ExistePorNombreAsync(nombre, ct);

            return Ok(ApiResponse<bool>.Ok(existe));
        }
    }
}
