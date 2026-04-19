using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Queries.Categorias
{
    public class CategoriaListQueryRepository
    {
        private readonly BookingAutoDbContext _context;

        public CategoriaListQueryRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaEntity>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Categorias
                .AsNoTracking()
                .Where(c => !c.es_eliminado)
                .ToListAsync(ct);
        }
    }
}