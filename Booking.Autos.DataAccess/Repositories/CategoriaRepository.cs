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
            // ✅ Buscar con tracking y actualizar solo los campos necesarios
            categoria.fecha_actualizacion = DateTime.UtcNow;


            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Categorias
                .FirstOrDefaultAsync(c => c.id_categoria == id && !c.es_eliminado, cancellationToken);

            if (existing == null) return;

            existing.es_eliminado = true;
            existing.fecha_eliminacion = DateTime.UtcNow;

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