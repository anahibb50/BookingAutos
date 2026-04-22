using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Clientes;
using Booking.Autos.DataManagement.Mappers;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.DataManagement.Services
{
    public class ClienteDataService : IClienteDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClienteDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS (PAGINADAS)
        // =========================

        public async Task<IReadOnlyList<ClienteDataModel>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Clientes.GetAllAsync(ct);

            return entities
                .Select(ClienteDataMapper.ToDataModel)
                .ToList();
        }

        public async Task<ClienteDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Clientes.GetByIdAsync(id);

            if (entity == null)
                return null;

            return ClienteDataMapper.ToDataModel(entity);
        }

        public async Task<ClienteDataModel?> GetByIdentificacionAsync(string identificacion, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Clientes.GetByIdentificacionAsync(identificacion, ct);
            if (entity == null) return null;
            return ClienteDataMapper.ToDataModel(entity);
        }

        public async Task<DataPagedResult<ClienteDataModel>> BuscarAsync(
            ClienteFiltroDataModel filtro,
            CancellationToken ct = default)
        {
            var query = (await _unitOfWork.Clientes.GetAllAsync(ct)).AsQueryable();

            // 🔍 FILTROS
            if (!string.IsNullOrWhiteSpace(filtro.Nombre))
                query = query.Where(x => x.cli_nombre.Contains(filtro.Nombre));

            if (!string.IsNullOrWhiteSpace(filtro.Apellido))
                query = query.Where(x => x.cli_apellido.Contains(filtro.Apellido));

            if (!string.IsNullOrWhiteSpace(filtro.Identificacion))
                query = query.Where(x => x.cli_ruc_ced.Contains(filtro.Identificacion));

            if (!string.IsNullOrWhiteSpace(filtro.TipoIdentificacion))
                query = query.Where(x => x.tipo_identificacion == filtro.TipoIdentificacion);

            if (filtro.IdCiudad.HasValue)
                query = query.Where(x => x.id_ciudad == filtro.IdCiudad.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(x => x.cli_estado == filtro.Estado);

            if (!string.IsNullOrWhiteSpace(filtro.Email))
                query = query.Where(x => x.cli_email != null && x.cli_email.Contains(filtro.Email));

            // 📊 TOTAL
            var totalRecords = query.Count();

            // 📄 PAGINACIÓN
            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .ToList();

            // 🔁 MAPEO
            var data = items.Select(ClienteDataMapper.ToDataModel);

            return new DataPagedResult<ClienteDataModel>(
                data,
                totalRecords,
                filtro.Page,
                filtro.PageSize
            );
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task<ClienteDataModel> CreateAsync(ClienteDataModel model, CancellationToken ct = default)
        {
            var entity = ClienteDataMapper.ToEntity(model);

            entity.cliente_guid = Guid.NewGuid();
            entity.fecha_registro_utc = DateTime.UtcNow;
            entity.es_eliminado = false;
            entity.creado_por_usuario = string.IsNullOrWhiteSpace(model.CreadoPorUsuario) ? "SYSTEM" : model.CreadoPorUsuario;
            entity.servicio_origen = string.IsNullOrWhiteSpace(model.ServicioOrigen) ? "API" : model.ServicioOrigen;
            entity.cli_estado = string.IsNullOrWhiteSpace(model.Estado) ? "ACT" : model.Estado;

            await _unitOfWork.Clientes.AddAsync(entity,ct);

            var creado = await _unitOfWork.Clientes.GetByIdAsync(entity.id_cliente, ct);
            return ClienteDataMapper.ToDataModel(creado!);
        }

        public async Task<ClienteDataModel> UpdateAsync(ClienteDataModel model, CancellationToken ct = default)
        {
            var existing = await _unitOfWork.Clientes.GetByIdAsync(model.Id);

            if (existing == null)
                throw new InvalidOperationException($"Cliente no encontrado: {model.Id}");

            existing.cli_nombre = model.Nombre;
            existing.cli_apellido = model.Apellido;
            existing.razon_social = model.RazonSocial;

            existing.tipo_identificacion = model.TipoIdentificacion;
            existing.cli_ruc_ced = model.Identificacion;

            existing.id_ciudad = model.IdCiudad;
            existing.cli_direccion = model.Direccion;
            existing.cli_genero = model.Genero;

            existing.cli_telefono = model.Telefono;
            existing.cli_email = model.Email;

            existing.cli_estado = model.Estado;

            existing.es_eliminado = model.EsEliminado;
            existing.fecha_eliminacion = model.FechaEliminacion;
            existing.modificado_por_usuario = model.ModificadoPorUsuario;
            existing.fecha_modificacion_utc = DateTime.UtcNow;
            existing.modificacion_ip = model.ModificacionIp;
            existing.servicio_origen = model.ServicioOrigen;

            await _unitOfWork.Clientes.UpdateAsync(existing, ct);
            

            return ClienteDataMapper.ToDataModel(existing);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Clientes.GetByIdAsync(id);

            if (entity == null)
                return false;

            entity.es_eliminado = true;
            entity.fecha_eliminacion = DateTime.UtcNow;

            await _unitOfWork.Clientes.UpdateAsync(entity,ct);
            

            return true;
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByIdentificacionAsync(string identificacion, CancellationToken ct = default)
        {
            return await _unitOfWork.Clientes.ExistsByIdentificacionAsync(identificacion, ct);
        }
    }
}
