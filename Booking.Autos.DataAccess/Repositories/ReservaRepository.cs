using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly BookingAutoDbContext _context;

        public ReservaRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS BASE
        // =========================

        public async Task<IEnumerable<ReservaEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(x => !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<ReservaEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .FirstOrDefaultAsync(x => x.id_reserva == id && !x.es_eliminado, cancellationToken);
        }

        public async Task<ReservaEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.guid_reserva == guid && !x.es_eliminado, cancellationToken);
        }

        // =========================
        // FILTROS
        // =========================

        public async Task<IEnumerable<ReservaEntity>> GetByClienteAsync(int idCliente, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(x => x.id_cliente == idCliente && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReservaEntity>> GetByVehiculoAsync(int idVehiculo, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(x => x.id_vehiculo == idVehiculo && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReservaEntity>> GetByEstadoAsync(string estado, CancellationToken cancellationToken = default)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Where(x => x.estado_reserva == estado && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        // =========================
        // DISPONIBILIDAD
        // =========================

        public async Task<bool> IsVehiculoDisponibleAsync(
            int idVehiculo,
            DateTime inicio,
            DateTime fin,
            CancellationToken cancellationToken = default)
        {
            return !await _context.Reservas
                .AnyAsync(x =>
                    x.id_vehiculo == idVehiculo &&
                    (x.estado_reserva == "PEN" || x.estado_reserva == "CON") &&
                    !x.es_eliminado &&
                    (
                        (inicio >= x.fecha_inicio && inicio < x.fecha_fin) ||
                        (fin > x.fecha_inicio && fin <= x.fecha_fin) ||
                        (inicio <= x.fecha_inicio && fin >= x.fecha_fin)
                    ),
                    cancellationToken);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(ReservaEntity reserva, CancellationToken cancellationToken = default)
        {
            reserva.guid_reserva = Guid.NewGuid();
            reserva.codigo_reserva = $"RES-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
            reserva.fecha_reserva_utc = DateTime.UtcNow;
            reserva.estado_reserva = "PEN";

            reserva.es_eliminado = false;

            await _context.Reservas.AddAsync(reserva, cancellationToken);
            
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Pon un breakpoint aquí o loggea esto
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"ERROR REAL: {innerMessage}", ex);
            }
        }

        public async Task UpdateAsync(ReservaEntity reserva, CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        // =========================
        // CASOS DE USO
        // =========================

        public async Task ConfirmarReservaAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Reservas
                .FirstOrDefaultAsync(x => x.id_reserva == id && !x.es_eliminado, cancellationToken);

            if (entity != null)
            {
                entity.estado_reserva = "CON";
                entity.fecha_confirmacion_utc = DateTime.UtcNow;
                entity.fecha_modificacion_utc = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task CancelarReservaAsync(int id, string motivo, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Reservas
                .FirstOrDefaultAsync(x => x.id_reserva == id && !x.es_eliminado, cancellationToken);

            if (entity != null)
            {
                entity.estado_reserva = "CAN";
                entity.fecha_cancelacion_utc = DateTime.UtcNow;
                entity.motivo_cancelacion = motivo;
                entity.fecha_modificacion_utc = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
