using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Conductores;
using Booking.Autos.DataManagement.Mappers;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.DataManagement.Services
{
    public class ConductorDataService : IConductorDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConductorDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IReadOnlyList<ConductorDataModel>> GetAllAsync(
            CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Conductores.GetAllAsync(ct);

            return entities
                .Select(ConductorDataMapper.ToDataModel)
                .ToList();
        }

        public async Task<ConductorDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Conductores.GetByIdAsync(id, ct);

            return entity == null
                ? null
                : ConductorDataMapper.ToDataModel(entity);
        }

        public async Task<ConductorDataModel?> GetByIdentificacionAsync(
            string identificacion,
            CancellationToken ct = default)
        {
            var conductores = await _unitOfWork.Conductores.GetAllAsync(ct);

            var entity = conductores.FirstOrDefault(x =>
                x.numero_identificacion == identificacion &&
                !x.es_eliminado);

            return entity == null
                ? null
                : ConductorDataMapper.ToDataModel(entity);
        }

        public async Task<ConductorDataModel?> GetByLicenciaAsync(
            string numeroLicencia,
            CancellationToken ct = default)
        {
            var conductores = await _unitOfWork.Conductores.GetAllAsync(ct);

            var entity = conductores.FirstOrDefault(x =>
                x.numero_licencia == numeroLicencia &&
                !x.es_eliminado);

            return entity == null
                ? null
                : ConductorDataMapper.ToDataModel(entity);
        }

        public async Task<DataPagedResult<ConductorDataModel>> BuscarAsync(
            ConductorFiltroDataModel filtro,
            CancellationToken ct = default)
        {
            var query = (await _unitOfWork.Conductores.GetAllAsync(ct)).AsQueryable();

            // 🔍 FILTROS
            if (!string.IsNullOrWhiteSpace(filtro.Nombre))
                query = query.Where(x => x.con_nombre1.Contains(filtro.Nombre));

            if (!string.IsNullOrWhiteSpace(filtro.Apellido))
                query = query.Where(x => x.con_apellido1.Contains(filtro.Apellido));

            if (!string.IsNullOrWhiteSpace(filtro.NumeroIdentificacion))
                query = query.Where(x => x.numero_identificacion.Contains(filtro.NumeroIdentificacion));

            if (!string.IsNullOrWhiteSpace(filtro.NumeroLicencia))
                query = query.Where(x => x.numero_licencia.Contains(filtro.NumeroLicencia));

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(x => x.estado_conductor == filtro.Estado);

            if (filtro.FechaVencimientoDesde.HasValue)
                query = query.Where(x => x.fecha_vencimiento_licencia >= filtro.FechaVencimientoDesde.Value);

            if (filtro.FechaVencimientoHasta.HasValue)
                query = query.Where(x => x.fecha_vencimiento_licencia <= filtro.FechaVencimientoHasta.Value);

            // 📊 TOTAL
            var totalRecords = query.Count();

            // 📄 PAGINACIÓN
            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .ToList();

            var data = items.Select(ConductorDataMapper.ToDataModel);

            return new DataPagedResult<ConductorDataModel>(
                data,
                totalRecords,
                filtro.Page,
                filtro.PageSize
            );
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task<ConductorDataModel> CreateAsync(
            ConductorDataModel model,
            CancellationToken ct = default)
        {
            var entity = ConductorDataMapper.ToEntity(model);

            entity.conductor_guid = Guid.NewGuid();
            entity.fecha_registro_utc = DateTime.UtcNow;
            entity.es_eliminado = false;

            await _unitOfWork.Conductores.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return ConductorDataMapper.ToDataModel(entity);
        }

        public async Task<ConductorDataModel> UpdateAsync(
            ConductorDataModel model,
            CancellationToken ct = default)
        {
            var existing = await _unitOfWork.Conductores.GetByIdAsync(model.Id);

            if (existing == null)
                throw new Exception("Conductor no encontrado");

            existing.codigo_conductor = model.Codigo;

            existing.tipo_identificacion = model.TipoIdentificacion;
            existing.numero_identificacion = model.NumeroIdentificacion;

            existing.con_nombre1 = model.Nombre1;
            existing.con_nombre2 = model.Nombre2;
            existing.con_apellido1 = model.Apellido1;
            existing.con_apellido2 = model.Apellido2;

            existing.numero_licencia = model.NumeroLicencia;
            existing.fecha_vencimiento_licencia = model.FechaVencimientoLicencia;
            existing.edad_conductor = model.Edad;

            existing.con_telefono = model.Telefono;
            existing.con_correo = model.Correo;

            existing.estado_conductor = model.Estado;

            existing.modificado_por_usuario = model.ModificadoPorUsuario;
            existing.fecha_modificacion_utc = DateTime.UtcNow;
            existing.modificado_desde_ip = model.ModificadoDesdeIp;

            existing.origen_registro = model.OrigenRegistro;
            existing.motivo_inhabilitacion = model.MotivoInhabilitacion;
            existing.es_eliminado = model.EsEliminado;
            existing.fecha_inhabilitacion_utc = model.FechaInhabilitacionUtc;

            await _unitOfWork.Conductores.UpdateAsync(existing,ct);

            return ConductorDataMapper.ToDataModel(existing);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Conductores.GetByIdAsync(id);

            if (entity == null)
                return false;

            entity.es_eliminado = true;
            entity.fecha_inhabilitacion_utc = DateTime.UtcNow;

            await _unitOfWork.Conductores.UpdateAsync(entity);

            return true;
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByIdentificacionAsync(string identificacion, CancellationToken ct = default)
        {
            return await _unitOfWork.Conductores.ExistsByCedulaAsync(identificacion, ct);
        }

        public async Task<bool> ExistsByLicenciaAsync(string numeroLicencia, CancellationToken ct = default)
        {
            return await _unitOfWork.Conductores.ExistsByLicenciaAsync(numeroLicencia, ct);
        }
    }
}