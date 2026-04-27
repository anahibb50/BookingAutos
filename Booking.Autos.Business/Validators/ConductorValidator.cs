using Booking.Autos.Business.DTOs.Conductor;

namespace Booking.Autos.Business.Validators
{
    public static class ConductorValidator
    {
        // =========================
        // CREACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarCreacion(CrearConductorRequest request)
        {
            var errors = new List<string>();

            // =========================
            // IDENTIFICACIÓN
            // =========================
            if (string.IsNullOrWhiteSpace(request.TipoIdentificacion))
                errors.Add("El tipo de identificación es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.TipoIdentificacion) &&
                request.TipoIdentificacion != "CEDULA" &&
                request.TipoIdentificacion != "RUC" &&
                request.TipoIdentificacion != "PASAPORTE")
                errors.Add("Tipo de identificación inválido.");

            if (string.IsNullOrWhiteSpace(request.NumeroIdentificacion))
                errors.Add("El número de identificación es obligatorio.");
            else
                ValidarNumeroIdentificacion(
                    errors,
                    request.TipoIdentificacion,
                    request.NumeroIdentificacion);

            // =========================
            // NOMBRES
            // =========================
            if (string.IsNullOrWhiteSpace(request.Nombre1))
                errors.Add("El primer nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Apellido1))
                errors.Add("El primer apellido es obligatorio.");

            // =========================
            // LICENCIA
            // =========================
            if (string.IsNullOrWhiteSpace(request.NumeroLicencia))
                errors.Add("El número de licencia es obligatorio.");

            if (request.FechaVencimientoLicencia <= DateTime.UtcNow.Date)
                errors.Add("La licencia debe estar vigente.");

            // =========================
            // EDAD (🔥 REGLA CLAVE)
            // =========================
            if (request.Edad < 18)
                errors.Add("El conductor debe ser mayor de edad (18+).");
            

            // =========================
            // CONTACTO
            // =========================
            if (string.IsNullOrWhiteSpace(request.Telefono))
                errors.Add("El teléfono es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Telefono) &&
                request.Telefono.Any(c => !char.IsDigit(c)))
                errors.Add("El teléfono solo debe contener números.");

            if (string.IsNullOrWhiteSpace(request.Correo))
                errors.Add("El correo es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Correo) &&
                !request.Correo.Contains("@"))
                errors.Add("El correo no tiene un formato válido.");

            return errors;
        }

        // =========================
        // ACTUALIZACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarConductorRequest request)
        {
            var errors = new List<string>();

            // =========================
            // ID
            // =========================
            if (request.Id <= 0)
                errors.Add("El id del conductor es inválido.");

            // =========================
            // NOMBRES
            // =========================
            if (string.IsNullOrWhiteSpace(request.Nombre1))
                errors.Add("El primer nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Apellido1))
                errors.Add("El primer apellido es obligatorio.");

            // =========================
            // IDENTIFICACIÓN
            // =========================
            if (string.IsNullOrWhiteSpace(request.TipoIdentificacion))
                errors.Add("El tipo de identificación es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.TipoIdentificacion) &&
                request.TipoIdentificacion != "CEDULA" &&
                request.TipoIdentificacion != "RUC" &&
                request.TipoIdentificacion != "PASAPORTE")
                errors.Add("Tipo de identificación inválido.");

            if (string.IsNullOrWhiteSpace(request.NumeroIdentificacion))
                errors.Add("El número de identificación es obligatorio.");
            else
                ValidarNumeroIdentificacion(
                    errors,
                    request.TipoIdentificacion,
                    request.NumeroIdentificacion);

            // =========================
            // LICENCIA
            // =========================
            if (string.IsNullOrWhiteSpace(request.NumeroLicencia))
                errors.Add("El número de licencia es obligatorio.");

            if (request.FechaVencimientoLicencia <= DateTime.UtcNow.Date)
                errors.Add("La licencia debe estar vigente.");

            // =========================
            // EDAD
            // =========================
            if (request.Edad < 18)
                errors.Add("El conductor debe ser mayor de edad (18+).");

            // =========================
            // CONTACTO
            // =========================
            if (string.IsNullOrWhiteSpace(request.Telefono))
                errors.Add("El teléfono es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Telefono) &&
                request.Telefono.Any(c => !char.IsDigit(c)))
                errors.Add("El teléfono solo debe contener números.");

            if (string.IsNullOrWhiteSpace(request.Correo))
                errors.Add("El correo es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Correo) &&
                !request.Correo.Contains("@"))
                errors.Add("El correo no tiene un formato válido.");

            // =========================
            // ESTADO
            // =========================
            if (string.IsNullOrWhiteSpace(request.Estado))
                errors.Add("El estado es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Estado) &&
                request.Estado != "ACT" &&
                request.Estado != "INA")
                errors.Add("Estado inválido.");

            return errors;
        }

        private static void ValidarNumeroIdentificacion(
            List<string> errors,
            string? tipoIdentificacion,
            string numeroIdentificacion)
        {
            var tipo = tipoIdentificacion?.Trim().ToUpperInvariant();
            var numero = numeroIdentificacion.Trim();

            if (tipo == "CEDULA")
            {
                if (numero.Length != 10)
                    errors.Add("La cédula debe tener exactamente 10 dígitos.");

                if (!numero.All(char.IsDigit))
                    errors.Add("La cédula solo debe contener números.");
            }
            else if (tipo == "RUC")
            {
                if (numero.Length != 13)
                    errors.Add("El RUC debe tener exactamente 13 dígitos.");

                if (!numero.All(char.IsDigit))
                    errors.Add("El RUC solo debe contener números.");
            }
            else if (tipo == "PASAPORTE")
            {
                if (numero.Length < 7 || numero.Length > 20)
                    errors.Add("El pasaporte debe tener entre 7 y 20 caracteres.");
            }
        }
    }
}