using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class LocalizacionRepository : ILocalizacionRepository
    {
        private readonly BookingAutoDbContext _context;

        public LocalizacionRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<LocalizacionEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Localizaciones
                .AsNoTracking()
                .Where(x => !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<LocalizacionEntity>> GetActivasAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Localizaciones
                .AsNoTracking()
                .Where(x => !x.es_eliminado && x.estado_localizacion == "ACT")
                .ToListAsync(cancellationToken);
        }

        public async Task<LocalizacionEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Localizaciones
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.id_localizacion == id && !x.es_eliminado, cancellationToken);
        }

        public async Task<LocalizacionEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await _context.Localizaciones
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.localizacion_guid == guid && !x.es_eliminado, cancellationToken);
        }

        public async Task<IEnumerable<LocalizacionEntity>> GetByCiudadIdAsync(int idCiudad, CancellationToken cancellationToken = default)
        {
            return await _context.Localizaciones
                .AsNoTracking()
                .Where(x => x.id_ciudad == idCiudad && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<LocalizacionEntity?> GetByNombreAsync(string nombre, CancellationToken cancellationToken = default)
        {
            return await _context.Localizaciones
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.nombre_localizacion == nombre && !x.es_eliminado, cancellationToken);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(LocalizacionEntity localizacion, CancellationToken cancellationToken = default)
        {
            localizacion.localizacion_guid = Guid.NewGuid();
            localizacion.fecha_registro_utc = DateTime.UtcNow;
            localizacion.estado_localizacion = "ACT";
            localizacion.es_eliminado = false;

            await _context.Localizaciones.AddAsync(localizacion, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(LocalizacionEntity localizacion, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Localizaciones
                .FirstOrDefaultAsync(x => x.id_localizacion == localizacion.id_localizacion, cancellationToken);

            if (existing == null)
                throw new Exception("Localización no encontrada");

            // 🔥 campos editables
            existing.codigo_localizacion = localizacion.codigo_localizacion;
            existing.nombre_localizacion = localizacion.nombre_localizacion;
            existing.id_ciudad = localizacion.id_ciudad;
            existing.direccion_localizacion = localizacion.direccion_localizacion;
            existing.telefono_contacto = localizacion.telefono_contacto;
            existing.correo_contacto = localizacion.correo_contacto;
            existing.horario_atencion = localizacion.horario_atencion;
            existing.zona_horaria = localizacion.zona_horaria;

            existing.estado_localizacion = localizacion.estado_localizacion;

            // auditoría
            existing.fecha_modificacion_utc = DateTime.UtcNow;
            existing.modificado_por_usuario = localizacion.modificado_por_usuario;
            existing.modificado_desde_ip = localizacion.modificado_desde_ip;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task InhabilitarAsync(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Localizaciones
                .FirstOrDefaultAsync(x => x.id_localizacion == id, cancellationToken);

            if (existing == null)
                throw new Exception("Localización no encontrada");

            // 🔥 inhabilitación lógica
            existing.estado_localizacion = "INA";
            existing.fecha_inhabilitacion_utc = DateTime.UtcNow;
            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByNombreEnCiudadAsync(string nombre, int idCiudad, CancellationToken cancellationToken = default)
        {
            return await _context.Localizaciones
                .AnyAsync(x =>
                    x.nombre_localizacion == nombre &&
                    x.id_ciudad == idCiudad &&
                    !x.es_eliminado,
                    cancellationToken);
        }
    }
}