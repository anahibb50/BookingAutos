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
                errors.Add("Fechas inválidas: la fecha de fin debe ser posterior a la fecha de inicio.");

            if (request.FechaInicio < DateTime.UtcNow.Date)
                errors.Add("No se puede reservar en fechas pasadas.");

            if (request.FechaInicio != default &&
                request.FechaFin != default)
            {
                var inicio = ConstruirFechaHora(request.FechaInicio, request.HoraInicio);
                var fin = ConstruirFechaHora(request.FechaFin, request.HoraFin);

                if (inicio >= fin)
                {
                    errors.Add("Fechas inválidas: la fecha/hora de fin debe ser posterior a la fecha/hora de inicio.");
                }
                else
                {
                    var diasCalculados = CalcularDiasPorDuracion(inicio, fin);
                    if (diasCalculados != request.CantidadDias)
                        errors.Add($"La cantidad de días no coincide con el rango seleccionado. Valor esperado: {diasCalculados}.");
                }
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

            if (request.IdCliente <= 0)
                errors.Add("El cliente es obligatorio.");

            if (request.IdVehiculo <= 0)
                errors.Add("El vehículo es obligatorio.");

            if (request.CantidadDias <= 0)
                errors.Add("La cantidad de días debe ser mayor a 0.");

            if (request.IdLocalizacionEntrega <= 0)
                errors.Add("La localización de entrega es obligatoria.");

            // =========================
            // FECHAS
            // =========================
            if (request.FechaInicio >= request.FechaFin)
                errors.Add("Fechas inválidas: la fecha de fin debe ser posterior a la fecha de inicio.");

            if (request.FechaInicio < DateTime.UtcNow.Date)
                errors.Add("No se puede modificar a fechas pasadas.");

            if (request.FechaInicio != default &&
                request.FechaFin != default)
            {
                var inicio = ConstruirFechaHora(request.FechaInicio, request.HoraInicio);
                var fin = ConstruirFechaHora(request.FechaFin, request.HoraFin);

                if (inicio >= fin)
                {
                    errors.Add("Fechas inválidas: la fecha/hora de fin debe ser posterior a la fecha/hora de inicio.");
                }
                else
                {
                    var diasCalculados = CalcularDiasPorDuracion(inicio, fin);
                    if (diasCalculados != request.CantidadDias)
                        errors.Add($"La cantidad de días no coincide con el rango seleccionado. Valor esperado: {diasCalculados}.");
                }
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

        private static DateTime ConstruirFechaHora(DateTime fecha, TimeSpan? hora)
        {
            var horaFinal = hora ?? fecha.TimeOfDay;
            return fecha.Date.Add(horaFinal);
        }

        private static int CalcularDiasPorDuracion(DateTime inicio, DateTime fin)
        {
            var duracion = fin - inicio;
            return (int)Math.Ceiling(duracion.TotalHours / 24d);
        }
    }
}
