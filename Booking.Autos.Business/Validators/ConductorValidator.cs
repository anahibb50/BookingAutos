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
                request.TipoIdentificacion != "CED" &&
                request.TipoIdentificacion != "PAS")
                errors.Add("Tipo de identificación inválido.");

            if (string.IsNullOrWhiteSpace(request.NumeroIdentificacion))
                errors.Add("El número de identificación es obligatorio.");

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
                request.TipoIdentificacion != "CED" &&
                request.TipoIdentificacion != "PAS")
                errors.Add("Tipo de identificación inválido.");

            if (string.IsNullOrWhiteSpace(request.NumeroIdentificacion))
                errors.Add("El número de identificación es obligatorio.");

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
    }
}