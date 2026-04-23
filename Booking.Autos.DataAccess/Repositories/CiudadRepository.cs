using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class CiudadRepository : ICiudadRepository
    {
        private readonly BookingAutoDbContext _context;

        public CiudadRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<CiudadEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Ciudades
                .AsNoTracking()
                .Where(x => !x.es_eliminado)
                .OrderBy(x => x.nombre_ciudad)
                .ToListAsync(cancellationToken);
        }

        public async Task<CiudadEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Ciudades
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.id_ciudad == id && !x.es_eliminado, cancellationToken);
        }

        public async Task<CiudadEntity?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default)
        {
            return await _context.Ciudades
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.nombre_ciudad == nombre && !x.es_eliminado, cancellationToken);
        }

        // =========================
        // CREATE
        // =========================

        public async Task AddAsync(CiudadEntity ciudad, CancellationToken cancellationToken = default)
        {
            ciudad.fecha_creacion = DateTime.UtcNow;
            ciudad.fecha_actualizacion = DateTime.UtcNow;
            ciudad.es_eliminado = false;

            await _context.Ciudades.AddAsync(ciudad, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // =========================
        // UPDATE
        // =========================

        public async Task UpdateAsync(CiudadEntity ciudad, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Ciudades
                .FirstOrDefaultAsync(x => x.id_ciudad == ciudad.id_ciudad && !x.es_eliminado, cancellationToken);

            if (existing == null)
                return;

            existing.nombre_ciudad = ciudad.nombre_ciudad;
            existing.codigo_postal = ciudad.codigo_postal;
            existing.id_pais = ciudad.id_pais;

            existing.estado_ciudad = ciudad.estado_ciudad;
            existing.origen_registro = ciudad.origen_registro;
            existing.es_eliminado = ciudad.es_eliminado;
            existing.fecha_eliminacion = ciudad.fecha_eliminacion;

            existing.fecha_actualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        // =========================
        // DELETE (SOFT DELETE)
        // =========================

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var ciudad = await _context.Ciudades
                .FirstOrDefaultAsync(x => x.id_ciudad == id && !x.es_eliminado, cancellationToken);

            if (ciudad == null)
                return;

            ciudad.es_eliminado = true;
            ciudad.fecha_eliminacion = DateTime.UtcNow;
            ciudad.fecha_actualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
