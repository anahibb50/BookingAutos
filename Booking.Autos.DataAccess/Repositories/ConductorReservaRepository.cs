using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class ConductorReservaRepository : IConductorReservaRepository
    {
        private readonly BookingAutoDbContext _context;

        public ConductorReservaRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS POR RELACIÓN
        // =========================

        public async Task<IEnumerable<ConductorReservaEntity>> GetByReservaIdAsync(int idReserva, CancellationToken cancellationToken = default)
        {
            return await _context.ConductoresReservas
                .AsNoTracking()
                .Where(x => x.id_reserva == idReserva && x.fecha_eliminacion == null)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ConductorReservaEntity>> GetByConductorIdAsync(int idConductor, CancellationToken cancellationToken = default)
        {
            return await _context.ConductoresReservas
                .AsNoTracking()
                .Where(x => x.id_conductor == idConductor && x.fecha_eliminacion == null)
                .ToListAsync(cancellationToken);
        }

        // =========================
        // LLAVE COMPUESTA
        // =========================

        public async Task<ConductorReservaEntity?> GetByIdsAsync(int idReserva, int idConductor, CancellationToken cancellationToken = default)
        {
            return await _context.ConductoresReservas
                .FirstOrDefaultAsync(x =>
                    x.id_reserva == idReserva &&
                    x.id_conductor == idConductor &&
                    x.fecha_eliminacion == null,
                    cancellationToken);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(ConductorReservaEntity entity, CancellationToken cancellationToken = default)
        {
            // 🔥 regla: si es principal → quitar principal anterior
            if (entity.es_principal)
            {
                var actuales = await _context.ConductoresReservas
                    .Where(x => x.id_reserva == entity.id_reserva && x.es_principal && x.fecha_eliminacion == null)
                    .ToListAsync(cancellationToken);

                foreach (var item in actuales)
                {
                    item.es_principal = false;
                    item.fecha_modificacion_utc = DateTime.UtcNow;
                }
            }

            entity.fecha_registro_utc = DateTime.UtcNow;
            entity.fecha_asignacion_utc = DateTime.UtcNow;
            entity.estado_asignacion = "PEN";

            await _context.ConductoresReservas.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(ConductorReservaEntity entity, CancellationToken cancellationToken = default)
        {
            var existing = await _context.ConductoresReservas
                .FirstOrDefaultAsync(x =>
                    x.id_reserva == entity.id_reserva &&
                    x.id_conductor == entity.id_conductor,
                    cancellationToken);

            if (existing == null)
                throw new Exception("Asignación no encontrada");

            // 🔥 si pasa a principal → quitar otros
            if (entity.es_principal && !existing.es_principal)
            {
                var actuales = await _context.ConductoresReservas
                    .Where(x => x.id_reserva == entity.id_reserva && x.es_principal && x.fecha_eliminacion == null)
                    .ToListAsync(cancellationToken);

                foreach (var item in actuales)
                {
                    item.es_principal = false;
                    item.fecha_modificacion_utc = DateTime.UtcNow;
                }
            }

            // actualizar campos
            existing.rol_conductor = entity.rol_conductor;
            existing.es_principal = entity.es_principal;
            existing.estado_asignacion = entity.estado_asignacion;
            existing.observaciones = entity.observaciones;

            existing.fecha_modificacion_utc = DateTime.UtcNow;
            existing.modificado_por_usuario = entity.modificado_por_usuario;
            existing.modificacion_ip = entity.modificacion_ip;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int idReserva, int idConductor, CancellationToken cancellationToken = default)
        {
            var existing = await _context.ConductoresReservas
                .FirstOrDefaultAsync(x =>
                    x.id_reserva == idReserva &&
                    x.id_conductor == idConductor,
                    cancellationToken);

            if (existing == null)
                throw new Exception("Asignación no encontrada");

            // 🔥 soft delete con fecha
            existing.fecha_eliminacion = DateTime.UtcNow;
            existing.estado_asignacion = "CAN";
            existing.fecha_desasignacion_utc = DateTime.UtcNow;
            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        // =========================
        // CONSULTAS DE NEGOCIO
        // =========================

        public async Task<ConductorReservaEntity?> GetConductorPrincipalByReservaIdAsync(int idReserva, CancellationToken cancellationToken = default)
        {
            return await _context.ConductoresReservas
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.id_reserva == idReserva &&
                    x.es_principal &&
                    x.fecha_eliminacion == null,
                    cancellationToken);
        }

        public async Task<bool> IsConductorAssignedToReservaAsync(int idReserva, int idConductor, CancellationToken cancellationToken = default)
        {
            return await _context.ConductoresReservas
                .AnyAsync(x =>
                    x.id_reserva == idReserva &&
                    x.id_conductor == idConductor &&
                    x.fecha_eliminacion == null,
                    cancellationToken);
        }
    }
}
