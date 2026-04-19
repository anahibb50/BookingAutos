using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class UsuarioAppRepository : IUsuarioAppRepository
    {
        private readonly BookingAutoDbContext _context;

        public UsuarioAppRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSULTAS BÁSICAS
        // =========================

        public async Task<IEnumerable<UsuarioAppEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosApp
                .AsNoTracking()
                .Where(u => !u.es_eliminado)
                .ToListAsync(cancellationToken);
        }

        public async Task<UsuarioAppEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosApp
                .FirstOrDefaultAsync(u => u.id_usuario == id && !u.es_eliminado, cancellationToken);
        }

        public async Task<UsuarioAppEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosApp
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.usuario_guid == guid && !u.es_eliminado, cancellationToken);
        }

        // =========================
        // AUTENTICACIÓN
        // =========================

        public async Task<UsuarioAppEntity?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosApp
                .FirstOrDefaultAsync(u =>
                    u.username == username &&
                    !u.es_eliminado,
                    cancellationToken);
        }

        public async Task<UsuarioAppEntity?> GetByCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosApp
                .FirstOrDefaultAsync(u =>
                    u.correo == correo &&
                    !u.es_eliminado,
                    cancellationToken);
        }

        // =========================
        // RELACIONES
        // =========================

        public async Task<UsuarioAppEntity?> GetByClienteIdAsync(int idCliente, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosApp
                .AsNoTracking()
                .Include(u => u.Cliente)
                .FirstOrDefaultAsync(u =>
                    u.id_cliente == idCliente &&
                    !u.es_eliminado,
                    cancellationToken);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(UsuarioAppEntity usuario, CancellationToken cancellationToken = default)
        {
            usuario.usuario_guid = Guid.NewGuid();
            usuario.fecha_registro_utc = DateTime.UtcNow;
            usuario.es_eliminado = false;

            await _context.UsuariosApp.AddAsync(usuario, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(UsuarioAppEntity usuario, CancellationToken cancellationToken = default)
        {
            _context.UsuariosApp.Update(usuario);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.UsuariosApp
                .FirstOrDefaultAsync(u => u.id_usuario == id, cancellationToken);

            if (entity != null)
            {
                entity.es_eliminado = true;
                entity.fecha_modificacion_utc = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosApp
                .AnyAsync(u =>
                    u.username == username &&
                    !u.es_eliminado,
                    cancellationToken);
        }

        public async Task<bool> ExistsByCorreoAsync(string correo, CancellationToken cancellationToken = default)
        {
            return await _context.UsuariosApp
                .AnyAsync(u =>
                    u.correo == correo &&
                    !u.es_eliminado,
                    cancellationToken);
        }
    }
}