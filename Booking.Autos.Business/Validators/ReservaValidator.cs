using Booking.Autos.Business.DTOs.Reserva;

namespace Booking.Autos.Business.Validators
{
    public static class ReservaValidator
    {
        // =========================
        // CREACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarCreacion(CrearReservaRequest request)
        {
            var errors = new List<string>();

            // =========================
            // RELACIONES
            // =========================
            if (request.IdCliente <= 0)
                errors.Add("El cliente es obligatorio.");

            if (request.IdVehiculo <= 0)
                errors.Add("El vehículo es obligatorio.");

            if (request.IdLocalizacionRecogida <= 0)
                errors.Add("La localización de recogida es obligatoria.");

            if (request.IdLocalizacionEntrega <= 0)
                errors.Add("La localización de entrega es obligatoria.");

            if (request.CantidadDias <= 0)
                errors.Add("La cantidad de días debe ser mayor a 0.");

            // =========================
            // FECHAS (🔥 CRÍTICO)
            // =========================
            if (request.FechaInicio == default)
                errors.Add("La fecha de inicio es obligatoria.");

            if (request.FechaFin == default)
                errors.Add("La fecha de fin es obligatoria.");

            if (request.FechaInicio >= request.FechaFin)
                errors.Add("La fecha de inicio debe ser menor a la fecha de fin.");

            if (request.FechaInicio < DateTime.UtcNow.Date)
                errors.Add("No se puede reservar en fechas pasadas.");

            // =========================
            // HORAS (OPCIONAL)
            // =========================
            if (request.HoraInicio.HasValue && request.HoraFin.HasValue)
            {
                if (request.HoraInicio >= request.HoraFin)
                    errors.Add("La hora de inicio debe ser menor a la hora de fin.");
            }

            // =========================
            // DESCRIPCIÓN
            // =========================
            if (!string.IsNullOrWhiteSpace(request.Descripcion) &&
                request.Descripcion.Length > 500)
                errors.Add("La descripción es demasiado larga.");

            return errors;
        }

        // =========================
        // ACTUALIZACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarReservaRequest request)
        {
            var errors = new List<string>();

            // =========================
            // ID
            // =========================
            if (request.Id <= 0)
                errors.Add("El id de la reserva es inválido.");


            // =========================
            // FECHAS
            // =========================
            if (request.FechaInicio >= request.FechaFin)
                errors.Add("La fecha de inicio debe ser menor a la fecha de fin.");

            if (request.FechaInicio < DateTime.UtcNow.Date)
                errors.Add("No se puede modificar a fechas pasadas.");

            // =========================
            // HORAS
            // =========================
            if (request.HoraInicio.HasValue && request.HoraFin.HasValue)
            {
                if (request.HoraInicio >= request.HoraFin)
                    errors.Add("La hora de inicio debe ser menor a la hora de fin.");
            }

            return errors;
        }

        // =========================
        // CONFIRMAR
        // =========================
        public static IReadOnlyCollection<string> ValidarConfirmacion(string estadoActual)
        {
            var errors = new List<string>();

            if (estadoActual != "PEN")
                errors.Add("Solo se pueden confirmar reservas en estado pendiente.");

            return errors;
        }

        // =========================
        // CANCELAR
        // =========================
        public static IReadOnlyCollection<string> ValidarCancelacion(string estadoActual, string motivo)
        {
            var errors = new List<string>();

            if (estadoActual == "CAN")
                errors.Add("La reserva ya está cancelada.");

            if (string.IsNullOrWhiteSpace(motivo))
                errors.Add("El motivo de cancelación es obligatorio.");

            return errors;
        }
    }
}