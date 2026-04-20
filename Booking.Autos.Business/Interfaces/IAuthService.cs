using Booking.Autos.Business.DTOs.Auth;


public interface IAuthService
{
    // =========================
    // AUTENTICACIÓN
    // =========================

    Task<LoginResponse> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default);

    // =========================
    // VALIDACIÓN
    // =========================

    Task<bool> ValidarTokenAsync(
        string token,
        CancellationToken cancellationToken = default);

    // =========================
    // REFRESH TOKEN (🔥 MUY IMPORTANTE)
    // =========================

    Task<LoginResponse> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    // =========================
    // LOGOUT (REVOCAR TOKEN)
    // =========================

    Task<bool> LogoutAsync(
        string token,
        CancellationToken cancellationToken = default);

   
}