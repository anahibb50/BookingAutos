using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.DTOs.Vehiculo;
using Booking.Autos.API.Models.Common;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/vehiculos")]
    [Authorize]
    public class VehiculoController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;

        public VehiculoController(IVehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        // ============================================================
        // 📌 CREAR
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearVehiculoRequest request,
            CancellationToken ct)
        {
            if (request == null)
                return BadRequest(new ApiErrorResponse("Request inválido"));

            var result = await _vehiculoService.CrearAsync(request, ct);

            return Ok(ApiResponse<VehiculoResponse>.Ok(result, "Vehículo creado"));
        }

        // ============================================================
        // ✏️ ACTUALIZAR
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            int id,
            [FromBody] ActualizarVehiculoRequest request,
            CancellationToken ct)
        {
            if (request == null)
                return BadRequest(new ApiErrorResponse("Request inválido"));

            request.Id = id;

            var result = await _vehiculoService.ActualizarAsync(request, ct);

            return Ok(ApiResponse<VehiculoResponse>.Ok(result, "Vehículo actualizado"));
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

            await _vehiculoService.EliminarLogicoAsync(id, usuario, ct);

            return Ok(ApiResponse<string>.Ok("OK", "Vehículo eliminado"));
        }

        // ============================================================
        // 🔍 OBTENER POR ID
        // ============================================================
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id, CancellationToken ct)
        {
            var result = await _vehiculoService.ObtenerPorIdAsync(id, ct);

            return Ok(ApiResponse<VehiculoResponse>.Ok(result));
        }

        // ============================================================
        // 🔍 POR PLACA (🔥 CLAVE)
        // ============================================================
        [AllowAnonymous]
        [HttpGet("por-placa")]
        public async Task<IActionResult> ObtenerPorPlaca(
            [FromQuery] string placa,
            CancellationToken ct)
        {
            var result = await _vehiculoService.ObtenerPorPlacaAsync(placa, ct);

            return Ok(ApiResponse<VehiculoResponse?>.Ok(result));
        }

        // ============================================================
        // 📄 LISTAR
        // ============================================================
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
        {
            var result = await _vehiculoService.ListarAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<VehiculoResponse>>.Ok(result));
        }

        // ============================================================
        // 🔍 BÚSQUEDA PAGINADA 🔥🔥🔥
        // ============================================================
        [AllowAnonymous]
        [HttpPost("buscar")]
        public async Task<IActionResult> Buscar(
            [FromBody] VehiculoFiltroRequest request,
            CancellationToken ct)
        {
            var result = await _vehiculoService.BuscarAsync(request, ct);

            return Ok(ApiResponse<DataPagedResult<VehiculoResponse>>.Ok(result));
        }

        // ============================================================
        // 🔎 FILTROS
        // ============================================================
        [AllowAnonymous]
        [HttpGet("por-marca/{idMarca}")]
        public async Task<IActionResult> PorMarca(int idMarca, CancellationToken ct)
        {
            var result = await _vehiculoService.ObtenerPorMarcaAsync(idMarca, ct);

            return Ok(ApiResponse<IReadOnlyList<VehiculoResponse>>.Ok(result));
        }

        [AllowAnonymous]
        [HttpGet("por-categoria/{idCategoria}")]
        public async Task<IActionResult> PorCategoria(int idCategoria, CancellationToken ct)
        {
            var result = await _vehiculoService.ObtenerPorCategoriaAsync(idCategoria, ct);

            return Ok(ApiResponse<IReadOnlyList<VehiculoResponse>>.Ok(result));
        }

        [AllowAnonymous]
        [HttpGet("disponibles")]
        public async Task<IActionResult> Disponibles(CancellationToken ct)
        {
            var result = await _vehiculoService.ListarDisponiblesAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<VehiculoResponse>>.Ok(result));
        }

        [AllowAnonymous]
        [HttpGet("por-precio")]
        public async Task<IActionResult> PorPrecio(
            [FromQuery] decimal min,
            [FromQuery] decimal max,
            CancellationToken ct)
        {
            var result = await _vehiculoService.ObtenerPorRangoPrecioAsync(min, max, ct);

            return Ok(ApiResponse<IReadOnlyList<VehiculoResponse>>.Ok(result));
        }

       

        // ============================================================
        // 🔥 OPERACIONES ESPECIALES
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPatch("{id}/kilometraje")]
        public async Task<IActionResult> ActualizarKilometraje(
            int id,
            [FromQuery] int nuevoKilometraje,
            CancellationToken ct)
        {
            var ok = await _vehiculoService.ActualizarKilometrajeAsync(id, nuevoKilometraje, ct);

            return Ok(ApiResponse<bool>.Ok(ok, "Kilometraje actualizado"));
        }

        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(
            int id,
            [FromQuery] string estado,
            CancellationToken ct)
        {
            var ok = await _vehiculoService.ActualizarEstadoAsync(id, estado, ct);

            return Ok(ApiResponse<bool>.Ok(ok, "Estado actualizado"));
        }

        // ============================================================
        // 🔍 VALIDACIÓN
        // ============================================================
        [Authorize(Roles = "ADMIN,VENDEDOR")]
        [HttpGet("existe-placa")]
        public async Task<IActionResult> ExistePorPlaca(
            [FromQuery] string placa,
            CancellationToken ct)
        {
            var existe = await _vehiculoService.ExistePorPlacaAsync(placa, ct);

            return Ok(ApiResponse<bool>.Ok(existe));
        }
    }
}
