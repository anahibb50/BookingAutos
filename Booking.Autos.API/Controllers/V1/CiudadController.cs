using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.DTOs.Catalogos.Ciudad;
using Booking.Autos.Business.DTOs.Ciudad;
using Booking.Autos.API.Models.Common;

namespace Booking.Autos.API.Controllers.V1.Catalogos
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/ciudades")]
    [Authorize]
    public class CiudadController : ControllerBase
    {
        private readonly ICiudadService _ciudadService;

        public CiudadController(ICiudadService ciudadService)
        {
            _ciudadService = ciudadService;
        }

        // ============================================================
        // 📌 CREAR
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearCiudadRequest request,
            CancellationToken ct)
        {
            var result = await _ciudadService.CrearAsync(request, ct);

            return Ok(ApiResponse<CiudadResponse>.Ok(result, "Ciudad creada"));
        }

        // ============================================================
        // ✏️ ACTUALIZAR
        // ============================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarCiudadRequest request,
            CancellationToken ct)
        {
            request.Id = id; // 🔥 importante

            var result = await _ciudadService.ActualizarAsync(request, ct);

            return Ok(ApiResponse<CiudadResponse>.Ok(result, "Ciudad actualizada"));
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

            await _ciudadService.EliminarLogicoAsync(id, usuario, ct);

            return Ok(ApiResponse<string>.Ok("OK", "Ciudad eliminada"));
        }

        // ============================================================
        // 🔍 OBTENER POR ID
        // ============================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(
            int id,
            CancellationToken ct)
        {
            var result = await _ciudadService.ObtenerPorIdAsync(id, ct);

            return Ok(ApiResponse<CiudadResponse>.Ok(result));
        }

        // ============================================================
        // 📄 LISTAR
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _ciudadService.ListarAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<CiudadResponse>>.Ok(result));
        }

        // ============================================================
        // 🌍 POR PAÍS (CLAVE 🔥)
        // ============================================================
        [HttpGet("por-pais/{idPais}")]
        public async Task<IActionResult> ObtenerPorPais(
            int idPais,
            CancellationToken ct)
        {
            var result = await _ciudadService.ObtenerPorPaisAsync(idPais, ct);

            return Ok(ApiResponse<IReadOnlyList<CiudadResponse>>.Ok(result));
        }

        // ============================================================
        // 🔍 VALIDACIÓN
        // ============================================================
        [HttpGet("existe")]
        public async Task<IActionResult> Existe(
            [FromQuery] string nombre,
            [FromQuery] int idPais,
            CancellationToken ct)
        {
            var existe = await _ciudadService.ExistePorNombreAsync(nombre, idPais, ct);

            return Ok(ApiResponse<bool>.Ok(existe));
        }
    }
}