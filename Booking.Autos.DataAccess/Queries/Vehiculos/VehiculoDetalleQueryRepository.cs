using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Queries.Vehiculos
{
    public class VehiculoDetalleQueryRepository
    {
        private readonly BookingAutoDbContext _context;

        public VehiculoDetalleQueryRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        public async Task<VehiculoEntity?> GetDetalleAsync(
            int idVehiculo,
            CancellationToken ct = default)
        {
            return await _context.Vehiculos
                .AsNoTracking()
                .Include(v => v.Marca)
                .Include(v => v.Categoria)
                .Include(v => v.Localizacion)
                .Include(v => v.Reservas) // por si business necesita disponibilidad
                .FirstOrDefaultAsync(v =>
                    v.id_vehiculo == idVehiculo &&
                    !v.es_eliminado,
                    ct);
        }
    }
}