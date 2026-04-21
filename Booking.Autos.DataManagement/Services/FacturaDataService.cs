using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Facturas;
using Booking.Autos.DataManagement.Mappers;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.DataManagement.Services
{
    public class FacturaDataService : IFacturaDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FacturaDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IReadOnlyList<FacturaDataModel>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Facturas.GetAllAsync(ct);

            return FacturaDataMapper.ToDataModelList(entities);
        }

        public async Task<FacturaDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Facturas.GetByIdAsync(id, ct);

            return entity == null
                ? null
                : FacturaDataMapper.ToDataModel(entity);
        }

        public async Task<IEnumerable<FacturaDataModel>> GetByClienteAsync(int idCliente, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Facturas.GetAllAsync(ct);

            return entities
                .Where(x => x.id_cliente == idCliente && !x.es_eliminado)
                .Select(FacturaDataMapper.ToDataModel);
        }

        public async Task<FacturaDataModel?> GetByReservaAsync(int idReserva, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Facturas.GetAllAsync(ct);

            var entity = entities.FirstOrDefault(x =>
                x.id_reserva == idReserva &&
                !x.es_eliminado);

            return entity == null
                ? null
                : FacturaDataMapper.ToDataModel(entity);
        }

        public async Task<DataPagedResult<FacturaDataModel>> BuscarAsync(
            FacturaFiltroDataModel filtro,
            CancellationToken ct = default)
        {
            var query = (await _unitOfWork.Facturas.GetAllAsync(ct)).AsQueryable();

            // 🔍 FILTROS
            if (filtro.IdCliente.HasValue)
                query = query.Where(x => x.id_cliente == filtro.IdCliente.Value);

            if (filtro.IdReserva.HasValue)
                query = query.Where(x => x.id_reserva == filtro.IdReserva.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(x => x.fac_estado == filtro.Estado);

            if (filtro.TotalMin.HasValue)
                query = query.Where(x => x.fac_total >= filtro.TotalMin.Value);

            if (filtro.TotalMax.HasValue)
                query = query.Where(x => x.fac_total <= filtro.TotalMax.Value);

            if (filtro.FechaCreacionDesde.HasValue)
                query = query.Where(x => x.fecha_creacion >= filtro.FechaCreacionDesde.Value);

            if (filtro.FechaCreacionHasta.HasValue)
                query = query.Where(x => x.fecha_creacion <= filtro.FechaCreacionHasta.Value);

            if (filtro.FechaAprobacionDesde.HasValue)
                query = query.Where(x => x.fecha_aprobacion >= filtro.FechaAprobacionDesde.Value);

            if (filtro.FechaAprobacionHasta.HasValue)
                query = query.Where(x => x.fecha_aprobacion <= filtro.FechaAprobacionHasta.Value);

            if (filtro.FechaAnulacionDesde.HasValue)
                query = query.Where(x => x.fecha_anulacion >= filtro.FechaAnulacionDesde.Value);

            if (filtro.FechaAnulacionHasta.HasValue)
                query = query.Where(x => x.fecha_anulacion <= filtro.FechaAnulacionHasta.Value);

            // 📊 TOTAL
            var totalRecords = query.Count();

            // 📄 PAGINACIÓN
            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .ToList();

            var data = items.Select(FacturaDataMapper.ToDataModel);

            return new DataPagedResult<FacturaDataModel>(
                data,
                totalRecords,
                filtro.Page,
                filtro.PageSize
            );
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task<FacturaDataModel> CreateAsync(FacturaDataModel model, CancellationToken ct = default)
        {
            var entity = FacturaDataMapper.ToEntity(model);

            entity.factura_guid = Guid.NewGuid();
            entity.fecha_creacion = DateTime.UtcNow;
            entity.fecha_actualizacion = DateTime.UtcNow;
            entity.es_eliminado = false;

            entity.fac_estado = "PEN"; // 🔥 siempre inicia pendiente

            await _unitOfWork.Facturas.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return FacturaDataMapper.ToDataModel(entity);
        }

        // =========================
        // ACCIONES DE NEGOCIO
        // =========================

        public async Task<bool> AprobarAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Facturas.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            if (entity.fac_estado != "PEN")
                return false; // 🔥 solo se aprueba si está pendiente

            entity.fac_estado = "APR";
            entity.fecha_aprobacion = DateTime.UtcNow;
            entity.fecha_actualizacion = DateTime.UtcNow;

            await _unitOfWork.Facturas.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> AnularAsync(int id, string motivo, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Facturas.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            if (entity.fac_estado == "ANU")
                return false;

            entity.fac_estado = "ANU";
            entity.fecha_anulacion = DateTime.UtcNow;
            entity.motivo_anulacion = motivo;
            entity.fecha_actualizacion = DateTime.UtcNow;

            await _unitOfWork.Facturas.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }
    }
}