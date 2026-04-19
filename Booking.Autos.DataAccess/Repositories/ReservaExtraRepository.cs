using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class ReservaExtraRepository : IReservaExtraRepository
    {
        private readonly BookingAutoDbContext _context;

        public ReservaExtraRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<ReservaExtraEntity>> GetByReservaAsync(int idReserva, CancellationToken cancellationToken = default)
        {
            return await _context.ReservaExtras
                .AsNoTracking()
                .Where(x => x.id_reserva == idReserva && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReservaExtraEntity>> GetByExtraAsync(int idExtra, CancellationToken cancellationToken = default)
        {
            return await _context.ReservaExtras
                .AsNoTracking()
                .Where(x => x.id_extra == idExtra && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<ReservaExtraEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await _context.ReservaExtras
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.r_x_e_guid == guid && !x.es_eliminado, cancellationToken);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(ReservaExtraEntity reservaExtra, CancellationToken cancellationToken = default)
        {
            reservaExtra.r_x_e_guid = Guid.NewGuid();
            reservaExtra.fecha_creacion = DateTime.UtcNow;
            reservaExtra.fecha_actualizacion = DateTime.UtcNow;
            reservaExtra.es_eliminado = false;
            reservaExtra.r_x_e_estado = "ACT";

            await _context.ReservaExtras.AddAsync(reservaExtra, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(ReservaExtraEntity reservaExtra, CancellationToken cancellationToken = default)
        {
            var existing = await _context.ReservaExtras
                .FirstOrDefaultAsync(x => x.id_reserva_extra == reservaExtra.id_reserva_extra, cancellationToken);

            if (existing == null)
                throw new Exception("ReservaExtra no encontrada");

            existing.id_extra = reservaExtra.id_extra;
            existing.r_x_e_cantidad = reservaExtra.r_x_e_cantidad;
            existing.r_x_e_valor_unitario = reservaExtra.r_x_e_valor_unitario;
            existing.r_x_e_subtotal = reservaExtra.r_x_e_subtotal;
            existing.r_x_e_estado = reservaExtra.r_x_e_estado;

            existing.fecha_actualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(int idReservaExtra, CancellationToken cancellationToken = default)
        {
            var existing = await _context.ReservaExtras
                .FirstOrDefaultAsync(x => x.id_reserva_extra == idReservaExtra, cancellationToken);

            if (existing == null)
                throw new Exception("ReservaExtra no encontrada");

            // 🔥 soft delete
            existing.es_eliminado = true;
            existing.fecha_eliminacion = DateTime.UtcNow;
            existing.fecha_actualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        // =========================
        // NEGOCIO / CÁLCULOS
        // =========================

        public async Task<decimal> GetSubtotalByReservaAsync(int idReserva, CancellationToken cancellationToken = default)
        {
            return await _context.ReservaExtras
                .Where(x => x.id_reserva == idReserva && !x.es_eliminado)
                .SumAsync(x => x.r_x_e_subtotal, cancellationToken);
        }
    }
}