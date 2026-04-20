using Booking.Autos.Business.DTOs.Factura;

namespace Booking.Autos.Business.Validators
{
    public static class FacturaValidator
    {
        // =========================
        // CREACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarCreacion(CrearFacturaRequest request)
        {
            var errors = new List<string>();

            // =========================
            // RELACIONES
            // =========================
            if (request.IdReserva <= 0)
                errors.Add("La reserva es obligatoria.");

            if (request.IdCliente <= 0)
                errors.Add("El cliente es obligatorio.");

            // =========================
            // DESCRIPCIÓN
            // =========================
            if (!string.IsNullOrWhiteSpace(request.Descripcion) &&
                request.Descripcion.Length > 500)
                errors.Add("La descripción no puede exceder 500 caracteres.");

            // =========================
            // ORIGEN
            // =========================
            if (!string.IsNullOrWhiteSpace(request.Origen) &&
                request.Origen.Length > 100)
                errors.Add("El origen no puede exceder 100 caracteres.");

            return errors;
        }

        // =========================
        // ACTUALIZACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarFacturaRequest request)
        {
            var errors = new List<string>();

            // =========================
            // ID
            // =========================
            if (request.Id <= 0)
                errors.Add("El id de la factura es inválido.");

            // =========================
            // DESCRIPCIÓN
            // =========================
            if (!string.IsNullOrWhiteSpace(request.Descripcion) &&
                request.Descripcion.Length > 500)
                errors.Add("La descripción no puede exceder 500 caracteres.");

            return errors;
        }

        // =========================
        // APROBACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarAprobacion(string estadoActual)
        {
            var errors = new List<string>();

            if (estadoActual != "PEN")
                errors.Add("Solo se pueden aprobar facturas en estado pendiente.");

            return errors;
        }

        // =========================
        // ANULACIÓN
        // =========================
        public static IReadOnlyCollection<string> ValidarAnulacion(string estadoActual, string motivo)
        {
            var errors = new List<string>();

            if (estadoActual == "ANU")
                errors.Add("La factura ya está anulada.");

            if (estadoActual != "PEN" && estadoActual != "APR")
                errors.Add("Solo se pueden anular facturas pendientes o aprobadas.");

            if (string.IsNullOrWhiteSpace(motivo))
                errors.Add("El motivo de anulación es obligatorio.");

            if (!string.IsNullOrWhiteSpace(motivo) && motivo.Length > 300)
                errors.Add("El motivo de anulación es demasiado largo.");

            return errors;
        }
    }
}