using Booking.Autos.Business.DTOs.Auth;
using Booking.Autos.Business.DTOs.Usuario;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Booking.Autos.Business.Validators
{
    public static class AuthValidator
    {
        private static readonly Regex CorreoRegex = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly Regex SoloDigitosRegex = new(@"^\d+$", RegexOptions.Compiled);

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

        // =========================
        // REGISTRO CLIENTE (formato)
        // =========================
        /// <summary>
        /// Validaciones de formato antes de crear usuario/cliente.
        /// No comprueba unicidad en base (eso lo hace el servicio).
        /// </summary>
        public static IReadOnlyCollection<string> ValidarFormatoRegistroCliente(CrearUsuarioRequest request)
        {
            var errors = new List<string>();

            if (request is null)
            {
                errors.Add("El cuerpo de la solicitud es obligatorio.");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(request.Username))
                errors.Add("El usuario es obligatorio.");
            else
            {
                if (TieneEspaciosAlInicioOFinal(request.Username))
                    errors.Add("El usuario no debe tener espacios al inicio ni al final.");

                var u = request.Username.Trim();
                if (u.Length < 3)
                    errors.Add("El usuario debe tener al menos 3 caracteres.");

                if (u.Any(char.IsWhiteSpace))
                    errors.Add("El usuario no debe contener espacios en blanco.");

                if (u.Length > 50)
                    errors.Add("El usuario no puede superar los 50 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(request.Correo))
                errors.Add("El correo es obligatorio.");
            else
            {
                if (TieneEspaciosAlInicioOFinal(request.Correo))
                    errors.Add("El correo no debe tener espacios al inicio ni al final.");

                var c = request.Correo.Trim();
                if (!CorreoRegex.IsMatch(c))
                    errors.Add("El correo electrónico no tiene un formato válido.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
                errors.Add("La contraseña es obligatoria.");
            else
            {
                if (TieneEspaciosAlInicioOFinal(request.Password))
                    errors.Add("La contraseña no debe tener espacios al inicio ni al final.");

                if (request.Password.Trim().Length < 6)
                    errors.Add("La contraseña debe tener al menos 6 caracteres.");
            }

            if (string.IsNullOrWhiteSpace(request.Identificacion))
                errors.Add("La identificación es obligatoria.");
            else
            {
                if (TieneEspaciosAlInicioOFinal(request.Identificacion))
                    errors.Add("La identificación no debe tener espacios al inicio ni al final.");

                if (request.Identificacion.Trim().Length > 20)
                    errors.Add("La identificación no puede superar los 20 caracteres.");
            }

            if (!string.IsNullOrWhiteSpace(request.Telefono))
            {
                if (TieneEspaciosAlInicioOFinal(request.Telefono))
                    errors.Add("El teléfono no debe tener espacios al inicio ni al final.");

                var t = request.Telefono.Trim();
                if (!SoloDigitosRegex.IsMatch(t))
                    errors.Add("El teléfono solo puede contener números.");

                if (t.Length > 15)
                    errors.Add("El teléfono no puede superar los 15 dígitos.");
            }

            if (!string.IsNullOrWhiteSpace(request.Nombre) && TieneEspaciosAlInicioOFinal(request.Nombre))
                errors.Add("El nombre no debe tener espacios al inicio ni al final.");

            if (!string.IsNullOrWhiteSpace(request.Apellido) && TieneEspaciosAlInicioOFinal(request.Apellido))
                errors.Add("El apellido no debe tener espacios al inicio ni al final.");

            if (!string.IsNullOrWhiteSpace(request.Direccion) && TieneEspaciosAlInicioOFinal(request.Direccion))
                errors.Add("La dirección no debe tener espacios al inicio ni al final.");

            return errors;
        }

        private static bool TieneEspaciosAlInicioOFinal(string value)
        {
            return !string.IsNullOrEmpty(value) && value.AsSpan().Trim().Length != value.Length;
        }
    }
}