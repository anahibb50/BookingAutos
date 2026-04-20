using Booking.Autos.Business.DTOs.Vehiculo;

namespace Booking.Autos.Business.Validators
{
    public static class VehiculoValidator
    {
        // =========================
        // CREACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarCreacion(CrearVehiculoRequest request)
        {
            var errors = new List<string>();

            // =========================
            // IDENTIFICACIÓN
            // =========================
            if (string.IsNullOrWhiteSpace(request.CodigoInterno))
                errors.Add("El código interno es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Placa))
                errors.Add("La placa es obligatoria.");

            // =========================
            // RELACIONES
            // =========================
            if (request.IdMarca <= 0)
                errors.Add("La marca es obligatoria.");

            if (request.IdCategoria <= 0)
                errors.Add("La categoría es obligatoria.");

            if (request.IdLocalizacion <= 0)
                errors.Add("La localización es obligatoria.");

            // =========================
            // INFO VEHÍCULO
            // =========================
            if (string.IsNullOrWhiteSpace(request.Modelo))
                errors.Add("El modelo es obligatorio.");

            if (request.AnioFabricacion < 1980 || request.AnioFabricacion > DateTime.UtcNow.Year + 1)
                errors.Add("El año de fabricación es inválido.");

            if (request.CapacidadPasajeros <= 0)
                errors.Add("La capacidad de pasajeros es inválida.");

            if (request.NumeroPuertas <= 0)
                errors.Add("El número de puertas es inválido.");

            // =========================
            // PRECIO
            // =========================
            if (request.PrecioBaseDia <= 0)
                errors.Add("El precio base debe ser mayor a 0.");

            return errors;
        }

        // =========================
        // ACTUALIZACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarVehiculoRequest request)
        {
            var errors = new List<string>();

            if (request.Id <= 0)
                errors.Add("El id del vehículo es inválido.");

            if (string.IsNullOrWhiteSpace(request.Modelo))
                errors.Add("El modelo es obligatorio.");

            if (request.PrecioBaseDia <= 0)
                errors.Add("El precio base debe ser mayor a 0.");

            if (request.IdLocalizacion <= 0)
                errors.Add("La localización es obligatoria.");

            if (string.IsNullOrWhiteSpace(request.Estado))
                errors.Add("El estado es obligatorio.");

            return errors;
        }

        // =========================
        // ESTADO
        // =========================
        public static IReadOnlyCollection<string> ValidarEstado(string estado)
        {
            var errors = new List<string>();

            if (estado != "DIS" && estado != "RES" && estado != "MAN")
                errors.Add("Estado de vehículo inválido.");

            return errors;
        }
    }
}