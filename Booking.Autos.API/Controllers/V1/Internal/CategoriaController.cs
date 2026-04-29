using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.DTOs.Catalogos.Categoria;
using Booking.Autos.API.Models.Common;

namespace Booking.Autos.API.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/categorias")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // ============================================================
        // 📌 CREAR
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearCategoriaRequest request,
            CancellationToken ct)
        {
            var result = await _categoriaService.CrearAsync(request, ct);

            return Ok(ApiResponse<CategoriaResponse>.Ok(result, "Categoría creada"));
        }

        // ============================================================
        // ✏️ ACTUALIZAR
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarCategoriaRequest request,
            CancellationToken ct)
        {
            request.Id = id; // 🔥 importante

            var result = await _categoriaService.ActualizarAsync(request, ct);

            return Ok(ApiResponse<CategoriaResponse>.Ok(result, "Categoría actualizada"));
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
            // 🔥 usuario desde JWT (opcional pero PRO)
            var usuario = User?.Identity?.Name ?? "system";

            await _categoriaService.EliminarLogicoAsync(id, usuario, ct);

            return Ok(ApiResponse<string>.Ok("OK", "Categoría eliminada"));
        }

        // ============================================================
        // 🔍 OBTENER POR ID
        // ============================================================
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(
            int id,
            CancellationToken ct)
        {
            var result = await _categoriaService.ObtenerPorIdAsync(id, ct);

            return Ok(ApiResponse<CategoriaResponse>.Ok(result));
        }

        // ============================================================
        // 📄 LISTAR
        // ============================================================
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _categoriaService.ListarAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<CategoriaResponse>>.Ok(result));
        }

        // ============================================================
        // 🔍 VALIDACIÓN (EXISTE)
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet("existe")]
        public async Task<IActionResult> Existe(
            [FromQuery] string nombre,
            CancellationToken ct)
        {
            var existe = await _categoriaService.ExistePorNombreAsync(nombre, ct);

            return Ok(ApiResponse<bool>.Ok(existe));
        }
    }
}
