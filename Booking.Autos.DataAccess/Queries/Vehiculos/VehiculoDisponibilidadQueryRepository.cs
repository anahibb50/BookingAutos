using Booking.Autos.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Queries.Vehiculos
{
    public class VehiculoDisponibilidadQueryRepository
    {
        private readonly BookingAutoDbContext _context;

        public VehiculoDisponibilidadQueryRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsDisponibleAsync(
            int idVehiculo,
            DateTime fechaInicio,
            DateTime fechaFin,
            int idLocalizacion,
            CancellationToken ct = default)
        {
            // 🔥 validamos que el vehículo esté en la localización correcta y activo
            var vehiculoValido = await _context.Vehiculos
                .AsNoTracking()
                .AnyAsync(v =>
                    v.id_vehiculo == idVehiculo &&
                    v.localizacion_actual == idLocalizacion &&
                    v.estado_vehiculo == "DIS" &&
                    !v.es_eliminado,
                    ct);

            if (!vehiculoValido)
                return false;

            // 🔥 verificamos que NO tenga reservas solapadas
            var tieneCruce = await _context.Reservas
                .AsNoTracking()
                .AnyAsync(r =>
                    r.id_vehiculo == idVehiculo &&
                    r.estado_reserva != "CAN" &&
                    !r.es_eliminado &&
                    (
                        (fechaInicio >= r.fecha_inicio && fechaInicio < r.fecha_fin) ||
                        (fechaFin > r.fecha_inicio && fechaFin <= r.fecha_fin) ||
                        (fechaInicio <= r.fecha_inicio && fechaFin >= r.fecha_fin)
                    ),
                    ct);

            return !tieneCruce;
        }
    }
}