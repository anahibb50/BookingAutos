using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Queries.Extras
{
    public class ExtraListQueryRepository
    {
        private readonly BookingAutoDbContext _context;

        public ExtraListQueryRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExtraEntity>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Extras
                .AsNoTracking()
                .Where(e =>
                    !e.es_eliminado &&
                    e.estado_extra == "ACT"
                )
                .ToListAsync(ct);
        }
    }
}