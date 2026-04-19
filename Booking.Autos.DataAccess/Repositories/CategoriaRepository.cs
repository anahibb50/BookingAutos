using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly BookingAutoDbContext _context;

        public CategoriaRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // --- Lectura ---

        public async Task<IReadOnlyList<CategoriaEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Categorias
                .AsNoTracking()
                .Where(c => !c.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<CategoriaEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.id_categoria == id && !c.es_eliminado, cancellationToken);
        }

        public async Task<CategoriaEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.categoria_guid == guid && !c.es_eliminado, cancellationToken);
        }

        public async Task<CategoriaEntity?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default)
        {
            return await _context.Categorias
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.nombre_categoria == nombre && !c.es_eliminado, cancellationToken);
        }

        // --- Escritura ---

        public async Task AddAsync(CategoriaEntity categoria, CancellationToken cancellationToken = default)
        {
            await _context.Categorias.AddAsync(categoria, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(CategoriaEntity categoria, CancellationToken cancellationToken = default)
        {
            _context.Entry(categoria).State = EntityState.Modified;

            _context.Entry(categoria).Property(x => x.fecha_creacion).IsModified = false;
            _context.Entry(categoria).Property(x => x.categoria_guid).IsModified = false;

            categoria.fecha_actualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var categoria = new CategoriaEntity { id_categoria = id };

            _context.Categorias.Attach(categoria);

            categoria.es_eliminado = true;
            categoria.fecha_eliminacion = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        // --- Validaciones ---

        public async Task<bool> ExistsByNombreAsync(string nombre, CancellationToken cancellationToken = default)
        {
            return await _context.Categorias
                .AnyAsync(c => c.nombre_categoria == nombre && !c.es_eliminado, cancellationToken);
        }

        public async Task<bool> TieneVehiculosAsociadosAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Vehiculos
                .AnyAsync(v => v.id_categoria == id && !v.es_eliminado, cancellationToken);
        }
    }
}