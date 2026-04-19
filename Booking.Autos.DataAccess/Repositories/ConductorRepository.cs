using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class ConductorRepository : IConductorRepository
    {
        private readonly BookingAutoDbContext _context;

        public ConductorRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<ConductorEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Conductores
                .AsNoTracking()
                .Where(x => !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<ConductorEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Conductores
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.id_conductor == id && !x.es_eliminado, cancellationToken);
        }

        public async Task<ConductorEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await _context.Conductores
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.conductor_guid == guid && !x.es_eliminado, cancellationToken);
        }

        public async Task<ConductorEntity?> GetByCedulaAsync(string cedula, CancellationToken cancellationToken = default)
        {
            return await _context.Conductores
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.numero_identificacion == cedula && !x.es_eliminado, cancellationToken);
        }

        public async Task<ConductorEntity?> GetByLicenciaAsync(string numLicencia, CancellationToken cancellationToken = default)
        {
            return await _context.Conductores
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.numero_licencia == numLicencia && !x.es_eliminado, cancellationToken);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(ConductorEntity conductor, CancellationToken cancellationToken = default)
        {
            conductor.conductor_guid = Guid.NewGuid();
            conductor.fecha_registro_utc = DateTime.UtcNow;
            conductor.es_eliminado = false;

            await _context.Conductores.AddAsync(conductor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(ConductorEntity conductor, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Conductores
                .FirstOrDefaultAsync(x => x.id_conductor == conductor.id_conductor, cancellationToken);

            if (existing == null)
                throw new Exception("Conductor no encontrado");

            // 🔥 SOLO CAMPOS EDITABLES
            existing.codigo_conductor = conductor.codigo_conductor;
            existing.tipo_identificacion = conductor.tipo_identificacion;
            existing.numero_identificacion = conductor.numero_identificacion;

            existing.con_nombre1 = conductor.con_nombre1;
            existing.con_nombre2 = conductor.con_nombre2;
            existing.con_apellido1 = conductor.con_apellido1;
            existing.con_apellido2 = conductor.con_apellido2;

            existing.numero_licencia = conductor.numero_licencia;
            existing.fecha_vencimiento_licencia = conductor.fecha_vencimiento_licencia;
            existing.edad_conductor = conductor.edad_conductor;

            existing.con_telefono = conductor.con_telefono;
            existing.con_correo = conductor.con_correo;

            existing.estado_conductor = conductor.estado_conductor;

            // Auditoría
            existing.fecha_modificacion_utc = DateTime.UtcNow;
            existing.modificado_por_usuario = conductor.modificado_por_usuario;
            existing.modificado_desde_ip = conductor.modificado_desde_ip;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Conductores
                .FirstOrDefaultAsync(x => x.id_conductor == id, cancellationToken);

            if (existing == null)
                throw new Exception("Conductor no encontrado");

            // 🔥 SOFT DELETE
            existing.es_eliminado = true;
            existing.estado_conductor = "INA";
            existing.fecha_inhabilitacion_utc = DateTime.UtcNow;
            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByCedulaAsync(string cedula, CancellationToken cancellationToken = default)
        {
            return await _context.Conductores
                .AnyAsync(x => x.numero_identificacion == cedula && !x.es_eliminado, cancellationToken);
        }

        public async Task<bool> ExistsByLicenciaAsync(string numLicencia, CancellationToken cancellationToken = default)
        {
            return await _context.Conductores
                .AnyAsync(x => x.numero_licencia == numLicencia && !x.es_eliminado, cancellationToken);
        }
    }
}