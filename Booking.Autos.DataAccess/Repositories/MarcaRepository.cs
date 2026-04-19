using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class MarcaRepository : IMarcaRepository
    {
        private readonly BookingAutoDbContext _context;

        public MarcaRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MarcaEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Marcas
                .Where(x => !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<MarcaEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Marcas
                .FirstOrDefaultAsync(x => x.id_marca == id && !x.es_eliminado, cancellationToken);
        }

        public async Task<MarcaEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await _context.Marcas
                .FirstOrDefaultAsync(x => x.marca_guid == guid && !x.es_eliminado, cancellationToken);
        }

        public async Task<MarcaEntity?> GetByNombreAsync(string nombreMarca, CancellationToken cancellationToken = default)
        {
            return await _context.Marcas
                .FirstOrDefaultAsync(x => x.nombre_marca == nombreMarca && !x.es_eliminado, cancellationToken);
        }

        public async Task AddAsync(MarcaEntity marca, CancellationToken cancellationToken = default)
        {
            await _context.Marcas.AddAsync(marca, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(MarcaEntity marca, CancellationToken cancellationToken = default)
        {
            _context.Marcas.Update(marca);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Marcas.FindAsync(new object[] { id }, cancellationToken);

            if (entity != null)
            {
                entity.es_eliminado = true;
                entity.fecha_eliminacion = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<bool> ExistsByNombreAsync(string nombreMarca, CancellationToken cancellationToken = default)
        {
            return await _context.Marcas
                .AnyAsync(x => x.nombre_marca == nombreMarca && !x.es_eliminado, cancellationToken);
        }

        public async Task<bool> TieneVehiculosAsociadosAsync(int idMarca, CancellationToken cancellationToken = default)
        {
            return await _context.Vehiculos
                .AnyAsync(x => x.id_marca == idMarca, cancellationToken);
        }
    }
}