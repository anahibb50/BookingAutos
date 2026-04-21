using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.DTOs.Rol;
using Booking.Autos.API.Models.Common;

namespace Booking.Autos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/roles")]
    [Authorize] // 🔥 importante en seguridad
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        // ============================================================
        // 📌 CREAR
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearRolRequest request,
            CancellationToken ct)
        {
            if (request == null)
                return BadRequest(new ApiErrorResponse("Request inválido"));

            var result = await _rolService.CrearAsync(request, ct);

            return Ok(ApiResponse<RolResponse>.Ok(result, "Rol creado"));
        }

        // ============================================================
        // ✏️ ACTUALIZAR
        // ============================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarRolRequest request,
            CancellationToken ct)
        {
            if (request == null)
                return BadRequest(new ApiErrorResponse("Request inválido"));

            request.IdRol = id;

            var result = await _rolService.ActualizarAsync(request, ct);

            return Ok(ApiResponse<RolResponse>.Ok(result, "Rol actualizado"));
        }

        // ============================================================
        // ❌ ELIMINAR
        // ============================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(
            int id,
            CancellationToken ct)
        {
            await _rolService.EliminarAsync(id, ct);

            return Ok(ApiResponse<string>.Ok("OK", "Rol eliminado"));
        }

        // ============================================================
        // 🔍 OBTENER POR ID
        // ============================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(
            int id,
            CancellationToken ct)
        {
            var result = await _rolService.ObtenerPorIdAsync(id, ct);

            return Ok(ApiResponse<RolResponse?>.Ok(result));
        }

        // ============================================================
        // 📄 LISTAR
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _rolService.ListarAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<RolResponse>>.Ok(result));
        }

        // ============================================================
        // 🔍 FILTRAR
        // ============================================================
        [HttpPost("filtrar")]
        public async Task<IActionResult> Filtrar(
            [FromBody] RolFiltroRequest request,
            CancellationToken ct)
        {
            var result = await _rolService.FiltrarAsync(request, ct);

            return Ok(ApiResponse<IReadOnlyList<RolResponse>>.Ok(result));
        }
    }
}