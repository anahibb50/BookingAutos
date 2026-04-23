using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class ExtraRepository : IExtraRepository
    {
        private readonly BookingAutoDbContext _context;

        public ExtraRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<ExtraEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Extras
                .AsNoTracking()
                .Where(x => !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ExtraEntity>> GetActivosAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Extras
                .AsNoTracking()
                .Where(x => !x.es_eliminado && x.estado_extra == "ACT")
                .ToListAsync(cancellationToken);
        }

        public async Task<ExtraEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Extras
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.id_extra == id && !x.es_eliminado, cancellationToken);
        }

        public async Task<ExtraEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await _context.Extras
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.extra_guid == guid && !x.es_eliminado, cancellationToken);
        }

        public async Task<IEnumerable<ExtraEntity>> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default)
        {
            return await _context.Extras
                .AsNoTracking()
                .Where(x => x.nombre_extra.Contains(nombre) && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(ExtraEntity extra, CancellationToken cancellationToken = default)
        {
            extra.extra_guid = Guid.NewGuid();
            extra.fecha_registro_utc = DateTime.UtcNow;
            extra.es_eliminado = false;

            await _context.Extras.AddAsync(extra, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(ExtraEntity extra, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Extras
                .FirstOrDefaultAsync(x => x.id_extra == extra.id_extra, cancellationToken);

            if (existing == null)
                throw new Exception("Extra no encontrado");

            // 🔥 campos editables
            existing.codigo_extra = extra.codigo_extra;
            existing.nombre_extra = extra.nombre_extra;
            existing.descripcion_extra = extra.descripcion_extra;
            existing.valor_fijo = extra.valor_fijo;
            existing.estado_extra = extra.estado_extra;
            existing.es_eliminado = extra.es_eliminado;
            existing.fecha_inhabilitacion_utc = extra.fecha_inhabilitacion_utc;
            existing.motivo_inhabilitacion = extra.motivo_inhabilitacion;
            existing.origen_registro = extra.origen_registro;

            // auditoría
            existing.fecha_modificacion_utc = DateTime.UtcNow;
            existing.modificado_por_usuario = extra.modificado_por_usuario;
            existing.modificado_desde_ip = extra.modificado_desde_ip;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Extras
                .FirstOrDefaultAsync(x => x.id_extra == id, cancellationToken);

            if (existing == null)
                throw new Exception("Extra no encontrado");

            // 🔥 soft delete
            existing.es_eliminado = true;
            existing.estado_extra = "INA";
            existing.fecha_inhabilitacion_utc = DateTime.UtcNow;
            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        // =========================
        // OPERACIÓN ESPECIAL
        // =========================

        public async Task UpdatePrecioAsync(int id, decimal nuevoPrecio, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Extras
                .FirstOrDefaultAsync(x => x.id_extra == id && !x.es_eliminado, cancellationToken);

            if (existing == null)
                throw new Exception("Extra no encontrado");

            existing.valor_fijo = nuevoPrecio;
            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
