using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.DTOs.Localizacion;
using Booking.Autos.API.Models.Common;

namespace Booking.Autos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/localizaciones")]
    [Authorize]
    public class LocalizacionController : ControllerBase
    {
        private readonly ILocalizacionService _localizacionService;

        public LocalizacionController(ILocalizacionService localizacionService)
        {
            _localizacionService = localizacionService;
        }

        // ============================================================
        // 📌 CREAR
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearLocalizacionRequest request,
            CancellationToken ct)
        {
            var result = await _localizacionService.CrearAsync(request, ct);

            return Ok(ApiResponse<LocalizacionResponse>.Ok(result, "Localización creada"));
        }

        // ============================================================
        // ✏️ ACTUALIZAR
        // ============================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarLocalizacionRequest request,
            CancellationToken ct)
        {
            request.Id = id;

            var result = await _localizacionService.ActualizarAsync(request, ct);

            return Ok(ApiResponse<LocalizacionResponse>.Ok(result, "Localización actualizada"));
        }

        // ============================================================
        // 🚫 INHABILITAR (🔥 NO DELETE)
        // ============================================================
        [HttpPatch("{id}/inhabilitar")]
        public async Task<IActionResult> Inhabilitar(
            int id,
            CancellationToken ct)
        {
            var usuario = User?.Identity?.Name ?? "system";

            await _localizacionService.InhabilitarAsync(id, usuario, ct);

            return Ok(ApiResponse<string>.Ok("OK", "Localización inhabilitada"));
        }

        // ============================================================
        // 🔍 OBTENER POR ID
        // ============================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(
            int id,
            CancellationToken ct)
        {
            var result = await _localizacionService.ObtenerPorIdAsync(id, ct);

            return Ok(ApiResponse<LocalizacionResponse>.Ok(result));
        }

        // ============================================================
        // 📄 LISTAR
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _localizacionService.ListarAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<LocalizacionResponse>>.Ok(result));
        }

        // ============================================================
        // 🌆 POR CIUDAD (🔥 CLAVE)
        // ============================================================
        [HttpGet("por-ciudad/{idCiudad}")]
        public async Task<IActionResult> ObtenerPorCiudad(
            int idCiudad,
            CancellationToken ct)
        {
            var result = await _localizacionService.ObtenerPorCiudadAsync(idCiudad, ct);

            return Ok(ApiResponse<IReadOnlyList<LocalizacionResponse>>.Ok(result));
        }

        // ============================================================
        // 🔍 POR NOMBRE
        // ============================================================
        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarPorNombre(
            [FromQuery] string nombre,
            CancellationToken ct)
        {
            var result = await _localizacionService.ObtenerPorNombreAsync(nombre, ct);

            return Ok(ApiResponse<LocalizacionResponse?>.Ok(result));
        }

        // ============================================================
        // 🔍 VALIDACIÓN
        // ============================================================
        [HttpGet("existe")]
        public async Task<IActionResult> Existe(
            [FromQuery] string nombre,
            [FromQuery] int idCiudad,
            CancellationToken ct)
        {
            var existe = await _localizacionService.ExistePorNombreEnCiudadAsync(nombre, idCiudad, ct);

            return Ok(ApiResponse<bool>.Ok(existe));
        }
    }
}