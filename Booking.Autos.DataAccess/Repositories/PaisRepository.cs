using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class PaisRepository : IPaisRepository
    {
        private readonly BookingAutoDbContext _context;

        public PaisRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<PaisEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Paises
                .Where(x => !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<PaisEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Paises
                .FirstOrDefaultAsync(x => x.id_pais == id && !x.es_eliminado, cancellationToken);
        }

        public async Task<PaisEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await _context.Paises
                .FirstOrDefaultAsync(x => x.pais_guid == guid && !x.es_eliminado, cancellationToken);
        }

        public async Task<PaisEntity?> GetByNombreAsync(string nombrePais, CancellationToken cancellationToken = default)
        {
            return await _context.Paises
                .FirstOrDefaultAsync(x => x.nombre_pais == nombrePais && !x.es_eliminado, cancellationToken);
        }

        public async Task<PaisEntity?> GetByCodigoIsoAsync(string codigoIso, CancellationToken cancellationToken = default)
        {
            return await _context.Paises
                .FirstOrDefaultAsync(x => x.codigo_iso == codigoIso && !x.es_eliminado, cancellationToken);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(PaisEntity pais, CancellationToken cancellationToken = default)
        {
            await _context.Paises.AddAsync(pais, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(PaisEntity pais, CancellationToken cancellationToken = default)
        {
            _context.Paises.Update(pais);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Paises.FindAsync(new object[] { id }, cancellationToken);

            if (entity != null)
            {
                entity.es_eliminado = true;
                entity.fecha_eliminacion = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> TieneCiudadesAsociadasAsync(int idPais, CancellationToken cancellationToken = default)
        {
            return await _context.Ciudades
                .AnyAsync(x => x.id_pais == idPais, cancellationToken);
        }
    }
}