using Booking.Autos.Business.DTOs.Auth;

namespace Booking.Autos.Business.Validators
{
    public static class AuthValidator
    {
        // =========================
        // LOGIN
        // =========================
        public static IReadOnlyCollection<string> ValidarLogin(LoginRequest request)
        {
            var errors = new List<string>();

            // =========================
            // USUARIO
            // =========================
            if (string.IsNullOrWhiteSpace(request.Username))
                errors.Add("El usuario es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Username) && request.Username.Length < 3)
                errors.Add("El usuario debe tener al menos 3 caracteres.");

            // =========================
            // PASSWORD
            // =========================
            if (string.IsNullOrWhiteSpace(request.Password))
                errors.Add("La contraseña es obligatoria.");

            if (!string.IsNullOrWhiteSpace(request.Password) && request.Password.Length < 6)
                errors.Add("La contraseña debe tener al menos 6 caracteres.");

            return errors;
        }
    }
}