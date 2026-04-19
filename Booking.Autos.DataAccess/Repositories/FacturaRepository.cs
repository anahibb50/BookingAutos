using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly BookingAutoDbContext _context;

        public FacturaRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<FacturaEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .Where(x => !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<FacturaEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.id_factura == id && !x.es_eliminado, cancellationToken);
        }

        public async Task<FacturaEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.factura_guid == guid && !x.es_eliminado, cancellationToken);
        }

        public async Task<IEnumerable<FacturaEntity>> GetByClienteAsync(int idCliente, CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .Where(x => x.id_cliente == idCliente && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<FacturaEntity?> GetByReservaAsync(int idReserva, CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.id_reserva == idReserva && !x.es_eliminado, cancellationToken);
        }

        public async Task<IEnumerable<FacturaEntity>> GetByRangoFechasAsync(DateTime inicio, DateTime fin, CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .Where(x => x.fecha_creacion >= inicio && x.fecha_creacion <= fin && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<FacturaEntity>> GetByEstadoAsync(string estado, CancellationToken cancellationToken = default)
        {
            return await _context.Facturas
                .AsNoTracking()
                .Where(x => x.fac_estado == estado && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(FacturaEntity factura, CancellationToken cancellationToken = default)
        {
            factura.factura_guid = Guid.NewGuid();
            factura.fecha_creacion = DateTime.UtcNow;
            factura.fecha_actualizacion = DateTime.UtcNow;
            factura.es_eliminado = false;
            factura.fac_estado = "PEN"; // 🔥 siempre inicia pendiente

            await _context.Facturas.AddAsync(factura, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(FacturaEntity factura, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Facturas
                .FirstOrDefaultAsync(x => x.id_factura == factura.id_factura && !x.es_eliminado, cancellationToken);

            if (existing == null)
                throw new Exception("Factura no encontrada");

            // 🔥 campos editables
            existing.fac_descripcion = factura.fac_descripcion;
            existing.origen_factura = factura.origen_factura;

            existing.fac_subtotal = factura.fac_subtotal;
            existing.fac_iva = factura.fac_iva;
            existing.fac_total = factura.fac_total;
            existing.id_cliente = factura.id_cliente;
            existing.id_reserva = factura.id_reserva;

            existing.fecha_actualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        // =========================
        // CASOS DE USO (NEGOCIO)
        // =========================

        public async Task AprobarAsync(int id, CancellationToken cancellationToken = default)
        {
            var factura = await _context.Facturas
                .FirstOrDefaultAsync(x => x.id_factura == id && !x.es_eliminado, cancellationToken);

            if (factura == null)
                throw new Exception("Factura no encontrada");

            // 🔥 regla: solo se puede aprobar si está pendiente
            if (factura.fac_estado != "PEN")
                throw new Exception("Solo se pueden aprobar facturas pendientes");

            factura.fac_estado = "APR";
            factura.fecha_aprobacion = DateTime.UtcNow;
            factura.fecha_actualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task AnularAsync(int id, string motivo, CancellationToken cancellationToken = default)
        {
            var factura = await _context.Facturas
                .FirstOrDefaultAsync(x => x.id_factura == id && !x.es_eliminado, cancellationToken);

            if (factura == null)
                throw new Exception("Factura no encontrada");

            // 🔥 regla: no puedes anular si ya está anulada
            if (factura.fac_estado == "ANU")
                throw new Exception("La factura ya está anulada");

            factura.fac_estado = "ANU";
            factura.fecha_anulacion = DateTime.UtcNow;
            factura.motivo_anulacion = motivo;
            factura.fecha_actualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}