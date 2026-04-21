using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booking.Autos.Business.Interfaces;
using Booking.Autos.Business.DTOs.Auth;
using Booking.Autos.API.Models.Common;

namespace Booking.Autos.API.Controllers.V1.Auth
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/auth")]
    [AllowAnonymous] // 🔥 todo este controller es público (login)
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ============================================================
        // 🔐 LOGIN
        // ============================================================
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            CancellationToken ct)
        {
            var result = await _authService.LoginAsync(request, ct);

            return Ok(ApiResponse<LoginResponse>.Ok(result, "Login exitoso"));
        }
    }
}