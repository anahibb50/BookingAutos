using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.DTOs.Catalogos.Pais;
using Booking.Autos.Business.DTOs.Pais;
using Booking.Autos.API.Models.Common;

namespace Booking.Autos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/paises")]
    [Authorize]
    public class PaisController : ControllerBase
    {
        private readonly IPaisService _paisService;

        public PaisController(IPaisService paisService)
        {
            _paisService = paisService;
        }

        // ============================================================
        // 📌 CREAR
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearPaisRequest request,
            CancellationToken ct)
        {
            if (request == null)
                return BadRequest(new ApiErrorResponse("Request inválido"));

            var result = await _paisService.CrearAsync(request, ct);

            return Ok(ApiResponse<PaisResponse>.Ok(result, "País creado"));
        }

        // ============================================================
        // ✏️ ACTUALIZAR
        // ============================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarPaisRequest request,
            CancellationToken ct)
        {
            if (request == null)
                return BadRequest(new ApiErrorResponse("Request inválido"));

            request.Id = id;

            var result = await _paisService.ActualizarAsync(request, ct);

            return Ok(ApiResponse<PaisResponse>.Ok(result, "País actualizado"));
        }

        // ============================================================
        // ❌ ELIMINAR LÓGICO
        // ============================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(
            int id,
            CancellationToken ct)
        {
            var usuario = User?.Identity?.Name ?? "anonymous";

            // 🔥 VALIDACIÓN DE NEGOCIO
            var tieneCiudades = await _paisService.TieneCiudadesAsociadasAsync(id, ct);

            if (tieneCiudades)
                return BadRequest(new ApiErrorResponse("No se puede eliminar el país porque tiene ciudades asociadas"));

            await _paisService.EliminarLogicoAsync(id, usuario, ct);

            return Ok(ApiResponse<string>.Ok("OK", "País eliminado"));
        }

        // ============================================================
        // 🔍 OBTENER POR ID
        // ============================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(
            int id,
            CancellationToken ct)
        {
            var result = await _paisService.ObtenerPorIdAsync(id, ct);

            return Ok(ApiResponse<PaisResponse>.Ok(result));
        }

        // ============================================================
        // 📄 LISTAR
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _paisService.ListarAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<PaisResponse>>.Ok(result));
        }

        // ============================================================
        // 🔍 POR NOMBRE
        // ============================================================
        [HttpGet("por-nombre")]
        public async Task<IActionResult> ObtenerPorNombre(
            [FromQuery] string nombre,
            CancellationToken ct)
        {
            var result = await _paisService.ObtenerPorNombreAsync(nombre, ct);

            return Ok(ApiResponse<PaisResponse?>.Ok(result));
        }

        // ============================================================
        // 🌍 POR CÓDIGO ISO (🔥 CLAVE)
        // ============================================================
        [HttpGet("por-iso")]
        public async Task<IActionResult> ObtenerPorIso(
            [FromQuery] string codigoIso,
            CancellationToken ct)
        {
            var result = await _paisService.ObtenerPorCodigoIsoAsync(codigoIso, ct);

            return Ok(ApiResponse<PaisResponse?>.Ok(result));
        }

        // ============================================================
        // 🔍 VALIDACIÓN NOMBRE
        // ============================================================
        [HttpGet("existe-nombre")]
        public async Task<IActionResult> ExistePorNombre(
            [FromQuery] string nombre,
            CancellationToken ct)
        {
            var existe = await _paisService.ExistePorNombreAsync(nombre, ct);

            return Ok(ApiResponse<bool>.Ok(existe));
        }

        // ============================================================
        // 🔍 VALIDACIÓN ISO
        // ============================================================
        [HttpGet("existe-iso")]
        public async Task<IActionResult> ExistePorIso(
            [FromQuery] string codigoIso,
            CancellationToken ct)
        {
            var existe = await _paisService.ExistePorCodigoIsoAsync(codigoIso, ct);

            return Ok(ApiResponse<bool>.Ok(existe));
        }
    }
}