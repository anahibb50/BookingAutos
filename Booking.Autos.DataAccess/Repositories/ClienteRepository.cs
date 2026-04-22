using Booking.Autos.DataAccess.Context;
using Booking.Autos.DataAccess.Entities;
using Booking.Autos.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Autos.DataAccess.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly BookingAutoDbContext _context;

        public ClienteRepository(BookingAutoDbContext context)
        {
            _context = context;
        }

        private IQueryable<ClienteEntity> BuildSafeQuery()
        {
            return _context.Clientes
                .AsNoTracking()
                .Select(x => new ClienteEntity
                {
                    id_cliente = x.id_cliente,
                    cliente_guid = x.cliente_guid,
                    cli_nombre = x.cli_nombre ?? string.Empty,
                    cli_apellido = x.cli_apellido ?? string.Empty,
                    razon_social = x.razon_social,
                    tipo_identificacion = x.tipo_identificacion ?? string.Empty,
                    cli_ruc_ced = x.cli_ruc_ced ?? string.Empty,
                    id_ciudad = x.id_ciudad,
                    cli_direccion = x.cli_direccion,
                    cli_genero = x.cli_genero,
                    cli_telefono = x.cli_telefono,
                    cli_email = x.cli_email,
                    cli_estado = x.cli_estado ?? "ACT",
                    creado_por_usuario = x.creado_por_usuario ?? "SYSTEM",
                    fecha_registro_utc = x.fecha_registro_utc,
                    modificado_por_usuario = x.modificado_por_usuario,
                    fecha_modificacion_utc = x.fecha_modificacion_utc,
                    modificacion_ip = x.modificacion_ip,
                    servicio_origen = x.servicio_origen,
                    es_eliminado = x.es_eliminado,
                    fecha_eliminacion = x.fecha_eliminacion
                });
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IEnumerable<ClienteEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await BuildSafeQuery()
                .Where(x => !x.es_eliminado)
                .OrderBy(x => x.id_cliente)
                .ToListAsync(cancellationToken);
        }

        public async Task<ClienteEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await BuildSafeQuery()
                .FirstOrDefaultAsync(x => x.id_cliente == id && !x.es_eliminado, cancellationToken);
        }

        public async Task<ClienteEntity?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default)
        {
            return await BuildSafeQuery()
                .FirstOrDefaultAsync(x => x.cliente_guid == guid && !x.es_eliminado, cancellationToken);
        }

        public async Task<ClienteEntity?> GetByIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default)
        {
            return await BuildSafeQuery()
                .FirstOrDefaultAsync(x => x.cli_ruc_ced == identificacion && !x.es_eliminado, cancellationToken);
        }

        public async Task<ClienteEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await BuildSafeQuery()
                .FirstOrDefaultAsync(x => x.cli_email == email && !x.es_eliminado, cancellationToken);
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task AddAsync(ClienteEntity cliente, CancellationToken cancellationToken = default)
        {
            cliente.fecha_registro_utc = DateTime.UtcNow;
            cliente.es_eliminado = false;

            await _context.Clientes.AddAsync(cliente, cancellationToken);
           
            
            await _context.SaveChangesAsync(cancellationToken);
        
        }

        public async Task UpdateAsync(ClienteEntity cliente, CancellationToken cancellationToken = default)
        {
            var existing = await _context.Clientes
                .FirstOrDefaultAsync(x => x.id_cliente == cliente.id_cliente && !x.es_eliminado, cancellationToken);

            if (existing == null)
                return;

            existing.cli_nombre = cliente.cli_nombre;
            existing.cli_apellido = cliente.cli_apellido;
            existing.razon_social = cliente.razon_social;

            existing.tipo_identificacion = cliente.tipo_identificacion;
            existing.cli_ruc_ced = cliente.cli_ruc_ced;

            existing.id_ciudad = cliente.id_ciudad;

            existing.cli_direccion = cliente.cli_direccion;
            existing.cli_genero = cliente.cli_genero;

            existing.cli_telefono = cliente.cli_telefono;
            existing.cli_email = cliente.cli_email;
            existing.es_eliminado = cliente.es_eliminado;      // 👈 clave
            existing.fecha_eliminacion = cliente.fecha_eliminacion;

            existing.cli_estado = cliente.cli_estado;

            existing.modificado_por_usuario = cliente.modificado_por_usuario;
            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(x => x.id_cliente == id && !x.es_eliminado, cancellationToken);

            if (cliente == null)
                return;

            cliente.es_eliminado = true;
            cliente.fecha_eliminacion = DateTime.UtcNow;
            cliente.fecha_modificacion_utc = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByIdentificacionAsync(string identificacion, CancellationToken cancellationToken = default)
        {
            return await _context.Clientes
                .AnyAsync(x => x.cli_ruc_ced == identificacion && !x.es_eliminado, cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Clientes
                .AnyAsync(x => x.cli_email == email && !x.es_eliminado, cancellationToken);
        }
    }
}
