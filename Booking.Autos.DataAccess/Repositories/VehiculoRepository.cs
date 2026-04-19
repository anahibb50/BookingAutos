using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly BookingAutoDbContext _context;

        public VehiculoRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // GESTIÓN DE FLOTA
        // =========================

        public async Task<IEnumerable<VehiculoEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Vehiculos
                .AsNoTracking()
                .Where(x => !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<VehiculoEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Vehiculos
                .FirstOrDefaultAsync(x => x.id_vehiculo == id && !x.es_eliminado, cancellationToken);
        }

        public async Task<VehiculoEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await _context.Vehiculos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.vehiculo_guid == guid && !x.es_eliminado, cancellationToken);
        }

        // =========================
        // BÚSQUEDAS
        // =========================

        public async Task<VehiculoEntity?> GetByPlacaAsync(string placa, CancellationToken cancellationToken = default)
        {
            return await _context.Vehiculos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.placa_vehiculo == placa && !x.es_eliminado, cancellationToken);
        }

        public async Task<IEnumerable<VehiculoEntity>> GetByMarcaIdAsync(int idMarca, CancellationToken cancellationToken = default)
        {
            return await _context.Vehiculos
                .AsNoTracking()
                .Where(x => x.id_marca == idMarca && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<VehiculoEntity>> GetByCategoriaIdAsync(int idCategoria, CancellationToken cancellationToken = default)
        {
            return await _context.Vehiculos
                .AsNoTracking()
                .Where(x => x.id_categoria == idCategoria && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        // =========================
        // DISPONIBILIDAD / OPERATIVO
        // =========================

        public async Task<IEnumerable<VehiculoEntity>> GetDisponiblesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Vehiculos
                .AsNoTracking()
                .Where(x => x.estado_vehiculo == "DIS" && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<VehiculoEntity>> GetByRangoPrecioAsync(decimal min, decimal max, CancellationToken cancellationToken = default)
        {
            return await _context.Vehiculos
                .AsNoTracking()
                .Where(x => x.precio_base_dia >= min && x.precio_base_dia <= max && !x.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(VehiculoEntity vehiculo, CancellationToken cancellationToken = default)
        {
            vehiculo.vehiculo_guid = Guid.NewGuid();
            vehiculo.fecha_registro_utc = DateTime.UtcNow;
            vehiculo.es_eliminado = false;

            await _context.Vehiculos.AddAsync(vehiculo, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(VehiculoEntity vehiculo, CancellationToken cancellationToken = default)
        {
            _context.Vehiculos.Update(vehiculo);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // =========================
        // OPERACIONES ESPECIALES
        // =========================

        public async Task UpdateKilometrajeAsync(int id, int nuevoKilometraje, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Vehiculos
                .FirstOrDefaultAsync(x => x.id_vehiculo == id && !x.es_eliminado, cancellationToken);

            if (entity != null)
            {
                entity.kilometraje_actual = nuevoKilometraje;
                entity.fecha_modificacion_utc = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateEstadoAsync(int id, string nuevoEstado, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Vehiculos
                .FirstOrDefaultAsync(x => x.id_vehiculo == id && !x.es_eliminado, cancellationToken);

            if (entity != null)
            {
                entity.estado_vehiculo = nuevoEstado;
                entity.fecha_modificacion_utc = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Vehiculos
                .FirstOrDefaultAsync(x => x.id_vehiculo == id, cancellationToken);

            if (entity != null)
            {
                entity.es_eliminado = true;
                entity.fecha_inhabilitacion_utc = DateTime.UtcNow;
                entity.fecha_modificacion_utc = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByPlacaAsync(string placa, CancellationToken cancellationToken = default)
        {
            return await _context.Vehiculos
                .AnyAsync(x => x.placa_vehiculo == placa && !x.es_eliminado, cancellationToken);
        }
    }
}