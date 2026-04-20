using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class UsuarioRolRepository : IUsuarioRolRepository
    {
        private readonly BookingAutoDbContext _context;

        public UsuarioRolRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<UsuarioRolEntity>> GetByUsuarioAsync(
            int idUsuario,
            CancellationToken ct = default)
        {
            return await _context.UsuariosRoles
                .AsNoTracking()
                .Include(x => x.Rol)
                .Where(x =>
                    x.id_usuario == idUsuario &&
                    !x.es_eliminado)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<UsuarioRolEntity>> GetByRolAsync(
            int idRol,
            CancellationToken ct = default)
        {
            return await _context.UsuariosRoles
                .AsNoTracking()
                .Include(x => x.UsuarioApp)
                .Where(x =>
                    x.id_rol == idRol &&
                    !x.es_eliminado)
                .ToListAsync(ct);
        }

        public async Task<UsuarioRolEntity?> GetAsync(
            int idUsuario,
            int idRol,
            CancellationToken ct = default)
        {
            return await _context.UsuariosRoles
                .FirstOrDefaultAsync(x =>
                    x.id_usuario == idUsuario &&
                    x.id_rol == idRol &&
                    !x.es_eliminado,
                    ct);
        }

        // 🔥 ESTE ES EL MÁS IMPORTANTE
        public async Task<List<string>> GetRolesByUsuarioAsync(
            int idUsuario,
            CancellationToken ct = default)
        {
            return await _context.UsuariosRoles
                .AsNoTracking()
                .Include(x => x.Rol)
                .Where(x =>
                    x.id_usuario == idUsuario &&
                    x.activo &&
                    !x.es_eliminado)
                .Select(x => x.Rol.nombre_rol)
                .ToListAsync(ct);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(
            UsuarioRolEntity entity,
            CancellationToken ct = default)
        {
            entity.fecha_registro_utc = DateTime.UtcNow;
            entity.es_eliminado = false;
            entity.activo = true;
            entity.estado_usuario_rol = "ACT";

            await _context.UsuariosRoles.AddAsync(entity, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(
            UsuarioRolEntity entity,
            CancellationToken ct = default)
        {
            var existing = await _context.UsuariosRoles
                .FirstOrDefaultAsync(x =>
                    x.id_usuario == entity.id_usuario &&
                    x.id_rol == entity.id_rol,
                    ct);

            if (existing == null)
                throw new Exception("Relación Usuario-Rol no encontrada");

            existing.estado_usuario_rol = entity.estado_usuario_rol;
            existing.activo = entity.activo;

            existing.modificado_por_usuario = entity.modificado_por_usuario;
            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
        }

        public async Task RemoveAsync(
            int idUsuario,
            int idRol,
            CancellationToken ct = default)
        {
            var existing = await _context.UsuariosRoles
                .FirstOrDefaultAsync(x =>
                    x.id_usuario == idUsuario &&
                    x.id_rol == idRol,
                    ct);

            if (existing == null)
                throw new Exception("Relación Usuario-Rol no encontrada");

            // 🔥 SOFT DELETE
            existing.es_eliminado = true;
            existing.activo = false;
            existing.estado_usuario_rol = "INA";
            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
        }
    }
}