using Booking.Autos.Business.DTOs.Factura;

namespace Booking.Autos.Business.Validators
{
    public static class FacturaValidator
    {
        public static IReadOnlyCollection<string> ValidarCreacion(CrearFacturaRequest request)
        {
            var errors = new List<string>();

            if (request.IdReserva <= 0)
                errors.Add("La reserva es obligatoria.");

            if (!string.IsNullOrWhiteSpace(request.Descripcion) &&
                request.Descripcion.Length > 500)
                errors.Add("La descripción no puede exceder 500 caracteres.");

            if (!string.IsNullOrWhiteSpace(request.Origen) &&
                request.Origen.Length > 100)
                errors.Add("El origen no puede exceder 100 caracteres.");

            return errors;
        }

        public static IReadOnlyCollection<string> ValidarActualizacion(ActualizarFacturaRequest request)
        {
            var errors = new List<string>();

            if (request.Id <= 0)
                errors.Add("El id de la factura es inválido.");

            if (!string.IsNullOrWhiteSpace(request.Descripcion) &&
                request.Descripcion.Length > 500)
                errors.Add("La descripción no puede exceder 500 caracteres.");

            return errors;
        }

        public static IReadOnlyCollection<string> ValidarAprobacion(string estadoActual)
        {
            var errors = new List<string>();

            if (estadoActual != "ABI")
                errors.Add("Solo se pueden aprobar facturas en estado abierta.");

            return errors;
        }

        public static IReadOnlyCollection<string> ValidarAnulacion(string estadoActual, string motivo)
        {
            var errors = new List<string>();

            if (estadoActual == "ANU")
                errors.Add("La factura ya está anulada.");

            if (estadoActual != "ABI" && estadoActual != "APR")
                errors.Add("Solo se pueden anular facturas abiertas o aprobadas.");

            if (string.IsNullOrWhiteSpace(motivo))
                errors.Add("El motivo de anulación es obligatorio.");

            if (!string.IsNullOrWhiteSpace(motivo) && motivo.Length > 300)
                errors.Add("El motivo de anulación es demasiado largo.");

            return errors;
        }
    }
}
