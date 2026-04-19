using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Queries.Localizaciones
{
    public class LocalizacionListQueryRepository
    {
        private readonly BookingAutoDbContext _context;

        public LocalizacionListQueryRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        public async Task<List<LocalizacionEntity>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Localizaciones
                .AsNoTracking()
                .Include(l => l.Ciudad)
                    .ThenInclude(c => c.Pais)
                .Where(l =>
                    !l.es_eliminado &&
                    l.estado_localizacion == "ACT"
                )
                .ToListAsync(ct);
        }
    }
}