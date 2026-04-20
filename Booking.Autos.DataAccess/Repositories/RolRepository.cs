using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly BookingAutoDbContext _context;

        public RolRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<RolEntity>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .Where(x => !x.es_eliminado)
                .OrderBy(x => x.nombre_rol)
                .ToListAsync(ct);
        }

        public async Task<RolEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(x =>
                    x.id_rol == id &&
                    !x.es_eliminado,
                    ct);
        }

        public async Task<RolEntity?> GetByNombreAsync(string nombre, CancellationToken ct = default)
        {
            return await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.nombre_rol == nombre &&
                    !x.es_eliminado,
                    ct);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(RolEntity rol, CancellationToken ct = default)
        {
            rol.rol_guid = Guid.NewGuid();
            rol.fecha_registro_utc = DateTime.UtcNow;
            rol.es_eliminado = false;
            rol.activo = true;
            rol.estado_rol = "ACT";

            await _context.Roles.AddAsync(rol, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(RolEntity rol, CancellationToken ct = default)
        {
            var existing = await _context.Roles
                .FirstOrDefaultAsync(x => x.id_rol == rol.id_rol, ct);

            if (existing == null)
                throw new Exception("Rol no encontrado");

            existing.nombre_rol = rol.nombre_rol;
            existing.descripcion_rol = rol.descripcion_rol;
            existing.estado_rol = rol.estado_rol;
            existing.activo = rol.activo;

            existing.modificado_por_usuario = rol.modificado_por_usuario;
            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var existing = await _context.Roles
                .FirstOrDefaultAsync(x => x.id_rol == id, ct);

            if (existing == null)
                throw new Exception("Rol no encontrado");

            // 🔥 SOFT DELETE
            existing.es_eliminado = true;
            existing.activo = false;
            existing.estado_rol = "INA";
            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
        }
    }
}