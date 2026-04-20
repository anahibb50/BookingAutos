using Booking.Autos.Business.DTOs.Cliente;

namespace Booking.Autos.Business.Validators
{
    public static class ClienteValidator
    {
        // =========================
        // CREACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarCreacion(CrearClienteRequest request)
        {
            var errors = new List<string>();

            // =========================
            // NOMBRES
            // =========================
            if (string.IsNullOrWhiteSpace(request.Nombre))
                errors.Add("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Apellido))
                errors.Add("El apellido es obligatorio.");

            // =========================
            // IDENTIFICACIÓN
            // =========================
            if (string.IsNullOrWhiteSpace(request.TipoIdentificacion))
                errors.Add("El tipo de identificación es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.TipoIdentificacion) &&
                request.TipoIdentificacion != "CED" &&
                request.TipoIdentificacion != "RUC" &&
                request.TipoIdentificacion != "PAS")
                errors.Add("El tipo de identificación es inválido.");

            if (string.IsNullOrWhiteSpace(request.Identificacion))
                errors.Add("La identificación es obligatoria.");

            // =========================
            // CIUDAD
            // =========================
            if (request.IdCiudad <= 0)
                errors.Add("Debe seleccionar una ciudad válida.");

            // =========================
            // GÉNERO
            // =========================
            if (string.IsNullOrWhiteSpace(request.Genero))
                errors.Add("El género es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Genero) &&
                request.Genero != "M" &&
                request.Genero != "F" &&
                request.Genero != "O")
                errors.Add("El género es inválido.");

            // =========================
            // EMAIL
            // =========================
            if (!string.IsNullOrWhiteSpace(request.Email) &&
                !request.Email.Contains("@"))
                errors.Add("El correo no tiene un formato válido.");

            // =========================
            // TELÉFONO
            // =========================
            if (!string.IsNullOrWhiteSpace(request.Telefono) &&
                request.Telefono.Any(c => !char.IsDigit(c)))
                errors.Add("El teléfono solo debe contener números.");

           

            return errors;
        }

        // =========================
        // ACTUALIZACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarClienteRequest request)
        {
            var errors = new List<string>();

            // =========================
            // ID
            // =========================
            if (request.Id <= 0)
                errors.Add("El id del cliente es inválido.");

            // =========================
            // NOMBRES
            // =========================
            if (string.IsNullOrWhiteSpace(request.Nombre))
                errors.Add("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Apellido))
                errors.Add("El apellido es obligatorio.");

            // =========================
            // IDENTIFICACIÓN
            // =========================
            if (string.IsNullOrWhiteSpace(request.TipoIdentificacion))
                errors.Add("El tipo de identificación es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.TipoIdentificacion) &&
                request.TipoIdentificacion != "CED" &&
                request.TipoIdentificacion != "RUC" &&
                request.TipoIdentificacion != "PAS")
                errors.Add("El tipo de identificación es inválido.");

            if (string.IsNullOrWhiteSpace(request.Identificacion))
                errors.Add("La identificación es obligatoria.");

            // =========================
            // CIUDAD
            // =========================
            if (request.IdCiudad <= 0)
                errors.Add("Debe seleccionar una ciudad válida.");

            // =========================
            // GÉNERO
            // =========================
            if (string.IsNullOrWhiteSpace(request.Genero))
                errors.Add("El género es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Genero) &&
                request.Genero != "M" &&
                request.Genero != "F" &&
                request.Genero != "O")
                errors.Add("El género es inválido.");

            // =========================
            // EMAIL
            // =========================
            if (!string.IsNullOrWhiteSpace(request.Email) &&
                !request.Email.Contains("@"))
                errors.Add("El correo no tiene un formato válido.");

            // =========================
            // TELÉFONO
            // =========================
            if (!string.IsNullOrWhiteSpace(request.Telefono) &&
                request.Telefono.Any(c => !char.IsDigit(c)))
                errors.Add("El teléfono solo debe contener números.");

            // =========================
            // ESTADO
            // =========================
            if (string.IsNullOrWhiteSpace(request.Estado))
                errors.Add("El estado es obligatorio.");

            if (!string.IsNullOrWhiteSpace(request.Estado) &&
                request.Estado != "ACT" &&
                request.Estado != "INA")
                errors.Add("El estado es inválido.");

            return errors;
        }
    }
}