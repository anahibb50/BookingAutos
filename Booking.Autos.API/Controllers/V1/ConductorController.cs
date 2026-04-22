using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.DTOs.Conductor;
using Booking.Autos.API.Models.Common;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/conductores")]
    [Authorize]
    public class ConductorController : ControllerBase
    {
        private readonly IConductorService _conductorService;

        public ConductorController(IConductorService conductorService)
        {
            _conductorService = conductorService;
        }

        // ============================================================
        // 📌 CREAR
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearConductorRequest request,
            CancellationToken ct)
        {
            var result = await _conductorService.CrearAsync(request, ct);

            return Ok(ApiResponse<ConductorResponse>.Ok(result, "Conductor creado"));
        }

        // ============================================================
        // ✏️ ACTUALIZAR
        // ============================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarConductorRequest request,
            CancellationToken ct)
        {
            request.Id = id;

            var result = await _conductorService.ActualizarAsync(request, ct);

            return Ok(ApiResponse<ConductorResponse>.Ok(result, "Conductor actualizado"));
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

            await _conductorService.EliminarLogicoAsync(id, usuario, ct);

            return Ok(ApiResponse<string>.Ok("OK", "Conductor eliminado"));
        }

        // ============================================================
        // 🔍 OBTENER POR ID
        // ============================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(
            int id,
            CancellationToken ct)
        {
            var result = await _conductorService.ObtenerPorIdAsync(id, ct);

            return Ok(ApiResponse<ConductorResponse>.Ok(result));
        }

        // ============================================================
        // 🔍 POR IDENTIFICACIÓN
        // ============================================================
        [HttpGet("por-identificacion/{identificacion}")]
        public async Task<IActionResult> ObtenerPorIdentificacion(
            string identificacion,
            CancellationToken ct)
        {
            var result = await _conductorService.ObtenerPorIdentificacionAsync(identificacion, ct);

            return Ok(ApiResponse<ConductorResponse?>.Ok(result));
        }

        // ============================================================
        // 🔍 POR LICENCIA (🔥 IMPORTANTE)
        // ============================================================
        [HttpGet("por-licencia/{numeroLicencia}")]
        public async Task<IActionResult> ObtenerPorLicencia(
            string numeroLicencia,
            CancellationToken ct)
        {
            var result = await _conductorService.ObtenerPorLicenciaAsync(numeroLicencia, ct);

            return Ok(ApiResponse<ConductorResponse?>.Ok(result));
        }

        // ============================================================
        // 📄 LISTAR
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _conductorService.ListarAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<ConductorResponse>>.Ok(result));
        }

        // ============================================================
        // 🔍 BÚSQUEDA PAGINADA 🔥🔥🔥
        // ============================================================
        [HttpPost("buscar")]
        public async Task<IActionResult> Buscar(
            [FromBody] ConductorFiltroRequest request,
            CancellationToken ct)
        {
            var result = await _conductorService.BuscarAsync(request, ct);

            return Ok(ApiResponse<DataPagedResult<ConductorResponse>>.Ok(result));
        }

        // ============================================================
        // 🔍 VALIDACIONES
        // ============================================================
        [HttpGet("existe-identificacion")]
        public async Task<IActionResult> ExistePorIdentificacion(
            [FromQuery] string identificacion,
            CancellationToken ct)
        {
            var existe = await _conductorService.ExistePorIdentificacionAsync(identificacion, ct);

            return Ok(ApiResponse<bool>.Ok(existe));
        }

        [HttpGet("existe-licencia")]
        public async Task<IActionResult> ExistePorLicencia(
            [FromQuery] string numeroLicencia,
            CancellationToken ct)
        {
            var existe = await _conductorService.ExistePorLicenciaAsync(numeroLicencia, ct);

            return Ok(ApiResponse<bool>.Ok(existe));
        }
    }
}