using Booking.Autos.Business.DTOs.Localizacion;

namespace Booking.Autos.Business.Validators
{
    public static class LocalizacionValidator
    {
        // =========================
        // CREACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarCreacion(CrearLocalizacionRequest request)
        {
            var errors = new List<string>();

            // =========================
            // IDENTIFICACIÓN
            // =========================
            

            if (string.IsNullOrWhiteSpace(request.Nombre))
                errors.Add("El nombre es obligatorio.");

            // =========================
            // RELACIÓN
            // =========================
            if (request.IdCiudad <= 0)
                errors.Add("Debe seleccionar una ciudad válida.");

            // =========================
            // DIRECCIÓN
            // =========================
            if (string.IsNullOrWhiteSpace(request.Direccion))
                errors.Add("La dirección es obligatoria.");

            if (!string.IsNullOrWhiteSpace(request.Direccion) && request.Direccion.Length > 300)
                errors.Add("La dirección no puede exceder 300 caracteres.");

            // =========================
            // TELÉFONO
            // =========================
            if (string.IsNullOrWhiteSpace(request.Telefono))
                errors.Add("El teléfono es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Telefono) &&
                request.Telefono.Any(c => !char.IsDigit(c)))
                errors.Add("El teléfono solo debe contener números.");

            // =========================
            // CORREO
            // =========================
            if (string.IsNullOrWhiteSpace(request.Correo))
                errors.Add("El correo es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Correo) &&
                !request.Correo.Contains("@"))
                errors.Add("El correo no tiene un formato válido.");

            // =========================
            // HORARIO
            // =========================
            if (string.IsNullOrWhiteSpace(request.HorarioAtencion))
                errors.Add("El horario de atención es obligatorio.");

            // =========================
            // ZONA HORARIA
            // =========================
            if (string.IsNullOrWhiteSpace(request.ZonaHoraria))
                errors.Add("La zona horaria es obligatoria.");

            return errors;
        }

        // =========================
        // ACTUALIZACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarLocalizacionRequest request)
        {
            var errors = new List<string>();

            // =========================
            // ID
            // =========================
            if (request.Id <= 0)
                errors.Add("El id de la localización es inválido.");

            // =========================
            // NOMBRE
            // =========================
            if (string.IsNullOrWhiteSpace(request.Nombre))
                errors.Add("El nombre es obligatorio.");

            // =========================
            // RELACIÓN
            // =========================
            if (request.IdCiudad <= 0)
                errors.Add("Debe seleccionar una ciudad válida.");

            // =========================
            // DIRECCIÓN
            // =========================
            if (string.IsNullOrWhiteSpace(request.Direccion))
                errors.Add("La dirección es obligatoria.");

            if (!string.IsNullOrWhiteSpace(request.Direccion) && request.Direccion.Length > 300)
                errors.Add("La dirección no puede exceder 300 caracteres.");

            // =========================
            // TELÉFONO
            // =========================
            if (string.IsNullOrWhiteSpace(request.Telefono))
                errors.Add("El teléfono es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Telefono) &&
                request.Telefono.Any(c => !char.IsDigit(c)))
                errors.Add("El teléfono solo debe contener números.");

            // =========================
            // CORREO
            // =========================
            if (string.IsNullOrWhiteSpace(request.Correo))
                errors.Add("El correo es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Correo) &&
                !request.Correo.Contains("@"))
                errors.Add("El correo no tiene un formato válido.");

            // =========================
            // HORARIO
            // =========================
            if (string.IsNullOrWhiteSpace(request.HorarioAtencion))
                errors.Add("El horario de atención es obligatorio.");

            // =========================
            // ZONA HORARIA
            // =========================
            if (string.IsNullOrWhiteSpace(request.ZonaHoraria))
                errors.Add("La zona horaria es obligatoria.");

            return errors;
        }

        // =========================
        // INHABILITAR
        // =========================
        public static IReadOnlyCollection<string> ValidarInhabilitacion(string estadoActual)
        {
            var errors = new List<string>();

            if (estadoActual == "INA")
                errors.Add("La localización ya está inhabilitada.");

            return errors;
        }
    }
}