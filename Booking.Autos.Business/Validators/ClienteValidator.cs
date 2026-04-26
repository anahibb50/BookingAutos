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
                request.TipoIdentificacion != "CEDULA" &&
                request.TipoIdentificacion != "RUC" &&
                request.TipoIdentificacion != "PASAPORTE")
                errors.Add("El tipo de identificación es inválido.");

            if (string.IsNullOrWhiteSpace(request.Identificacion))
                errors.Add("La identificación es obligatoria.");

            if (!string.IsNullOrWhiteSpace(request.Identificacion) &&
                !string.IsNullOrWhiteSpace(request.TipoIdentificacion))
            {
                switch (request.TipoIdentificacion)
                {
                    case "CEDULA":
                        if (request.Identificacion.Length != 10)
                            errors.Add("La cédula debe tener exactamente 10 dígitos.");
                        if (!request.Identificacion.All(char.IsDigit))
                            errors.Add("La cédula solo debe contener números.");
                        break;

                    case "RUC":
                        if (request.Identificacion.Length != 13)
                            errors.Add("El RUC debe tener exactamente 13 dígitos.");
                        if (!request.Identificacion.All(char.IsDigit))
                            errors.Add("El RUC solo debe contener números.");
                        break;

                    case "PASAPORTE":
                        if (request.Identificacion.Length < 5 || request.Identificacion.Length > 20)
                            errors.Add("El pasaporte debe tener entre 5 y 20 caracteres.");
                        break;
                }
            }

            // =========================
            // CIUDAD
            // =========================
            if (request.IdCiudad <= 0)
                errors.Add("Debe seleccionar una ciudad válida.");

            // =========================
            // GÉNERO
            // =========================
            if (!string.IsNullOrWhiteSpace(request.Genero) &&
                request.Genero != "M" &&
                request.Genero != "F")
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

            if (!string.IsNullOrWhiteSpace(request.Telefono) &&
                (request.Telefono.Length < 7 || request.Telefono.Length > 10))
                errors.Add("El teléfono debe tener entre 7 y 10 dígitos.");


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
                request.TipoIdentificacion != "CEDULA" &&
                request.TipoIdentificacion != "RUC" &&
                request.TipoIdentificacion != "PASAPORTE")
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
            if (!string.IsNullOrWhiteSpace(request.Genero) &&
                request.Genero != "M" &&
                request.Genero != "F")
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

            if (!string.IsNullOrWhiteSpace(request.Telefono) &&
                (request.Telefono.Length < 7 || request.Telefono.Length > 10))
                errors.Add("El teléfono debe tener entre 7 y 10 dígitos.");

            return errors;
        }
    }
}