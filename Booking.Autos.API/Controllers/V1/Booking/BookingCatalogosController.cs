using Asp.Versioning;
using Booking.Autos.API.Models.Booking;
using Booking.Autos.API.Models.Common;
using Booking.Autos.Business.DTOs.Catalogos.Categoria;
using Booking.Autos.Business.DTOs.Catalogos.Marca;
using Booking.Autos.Business.DTOs.Extra;
using Booking.Autos.Business.DTOs.Localizacion;
using Booking.Autos.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Autos.API.Controllers.V1.Booking
{
    /// <summary>
    /// Endpoints públicos de catálogos para integración con Booking / OTA.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/booking")]
    public class BookingCatalogosController : ControllerBase
    {
        private readonly ILocalizacionService _localizacionService;
        private readonly ICategoriaService _categoriaService;
        private readonly IMarcaService _marcaService;
        private readonly IExtraService _extraService;

        public BookingCatalogosController(
            ILocalizacionService localizacionService,
            ICategoriaService categoriaService,
            IMarcaService marcaService,
            IExtraService extraService)
        {
            _localizacionService = localizacionService;
            _categoriaService = categoriaService;
            _marcaService = marcaService;
            _extraService = extraService;
        }

        /// <summary>
        /// Endpoint 4: Listar localizaciones (sucursales) con paginación.
        /// GET /api/v1/localizaciones?idCiudad=1&page=1&limit=20
        /// </summary>
        [HttpGet("localizaciones")]
        public async Task<IActionResult> GetLocalizaciones(
            [FromQuery] BookingLocalizacionesRequest request,
            CancellationToken ct)
        {
            var result = request.IdCiudad.HasValue
                ? await _localizacionService.ObtenerPorCiudadAsync(request.IdCiudad.Value, ct)
                : await _localizacionService.ListarAsync(ct);

            return Ok(ApiResponse<IReadOnlyList<LocalizacionResponse>>.Ok(result));
        }

        /// <summary>
        /// Endpoint 5: Detalle de una localización específica.
        /// GET /api/v1/localizaciones/{localizacionId}
        /// </summary>
        [HttpGet("localizaciones/{localizacionId:int}")]
        public async Task<IActionResult> GetLocalizacionDetalle(
            int localizacionId,
            CancellationToken ct)
        {
            var result = await _localizacionService.ObtenerPorIdAsync(localizacionId, ct);
            return Ok(ApiResponse<LocalizacionResponse>.Ok(result));
        }

        /// <summary>
        /// Endpoint 6: Listar categorías de vehículos.
        /// GET /api/v1/categorias
        /// </summary>
        [HttpGet("categorias")]
        public async Task<IActionResult> GetCategorias(CancellationToken ct)
        {
            var result = await _categoriaService.ListarAsync(ct);
            return Ok(ApiResponse<IReadOnlyList<CategoriaResponse>>.Ok(result));
        }

        /// <summary>
        /// Endpoint adicional: Listar marcas de vehículos.
        /// GET /api/v1/marcas
        /// </summary>
        [HttpGet("marcas")]
        public async Task<IActionResult> GetMarcas(CancellationToken ct)
        {
            var result = await _marcaService.ListarAsync(ct);
            return Ok(ApiResponse<IReadOnlyList<MarcaResponse>>.Ok(result));
        }

        /// <summary>
        /// Endpoint 7: Listar extras disponibles con precio fijo.
        /// GET /api/v1/extras
        /// </summary>
        [HttpGet("extras")]
        public async Task<IActionResult> GetExtras(CancellationToken ct)
        {
            var result = await _extraService.ListarActivosAsync(ct);
            return Ok(ApiResponse<IReadOnlyList<ExtraResponse>>.Ok(result));
        }
    }
}
