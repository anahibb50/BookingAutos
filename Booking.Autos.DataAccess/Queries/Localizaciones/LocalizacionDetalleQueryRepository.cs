using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Queries.Localizaciones
{
    public class LocalizacionDetalleQueryRepository
    {
        private readonly BookingAutoDbContext _context;

        public LocalizacionDetalleQueryRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        public async Task<LocalizacionEntity?> GetByIdAsync(
            int idLocalizacion,
            CancellationToken ct = default)
        {
            return await _context.Localizaciones
                .AsNoTracking()
                .Include(l => l.Ciudad)
                    .ThenInclude(c => c.Pais)
                .Include(l => l.Vehiculos) // opcional, útil para business
                .FirstOrDefaultAsync(l =>
                    l.id_localizacion == idLocalizacion &&
                    !l.es_eliminado,
                    ct);
        }
    }
}