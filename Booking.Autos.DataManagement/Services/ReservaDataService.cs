using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Reservas;
using Booking.Autos.DataManagement.Mappers;
using Microservicio.Clientes.DataManagement.Models;

namespace Booking.Autos.DataManagement.Services
{
    public class ReservaDataService : IReservaDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservaDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IReadOnlyList<ReservaDataModel>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Reservas.GetAllAsync(ct);

            return ReservaDataMapper.ToDataModelList(entities);
        }

        public async Task<ReservaDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Reservas.GetByIdAsync(id, ct);

            return entity == null ? null : ReservaDataMapper.ToDataModel(entity);
        }

        public async Task<IEnumerable<ReservaDataModel>> GetByClienteAsync(int idCliente, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Reservas.GetAllAsync(ct);

            return entities
                .Where(x => x.id_cliente == idCliente && !x.es_eliminado)
                .Select(ReservaDataMapper.ToDataModel);
        }

        public async Task<IEnumerable<ReservaDataModel>> GetByVehiculoAsync(int idVehiculo, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Reservas.GetAllAsync(ct);

            return entities
                .Where(x => x.id_vehiculo == idVehiculo && !x.es_eliminado)
                .Select(ReservaDataMapper.ToDataModel);
        }

        public async Task<DataPagedResult<ReservaDataModel>> BuscarAsync(
            ReservaFiltroDataModel filtro,
            CancellationToken ct = default)
        {
            var query = (await _unitOfWork.Reservas.GetAllAsync(ct)).AsQueryable();

            if (filtro.IdCliente.HasValue)
                query = query.Where(x => x.id_cliente == filtro.IdCliente);

            if (filtro.IdVehiculo.HasValue)
                query = query.Where(x => x.id_vehiculo == filtro.IdVehiculo);

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(x => x.estado_reserva == filtro.Estado);

            if (filtro.FechaInicioDesde.HasValue)
                query = query.Where(x => x.fecha_inicio >= filtro.FechaInicioDesde);

            if (filtro.FechaInicioHasta.HasValue)
                query = query.Where(x => x.fecha_inicio <= filtro.FechaInicioHasta);

            if (!string.IsNullOrWhiteSpace(filtro.CodigoReserva))
                query = query.Where(x => x.codigo_reserva.Contains(filtro.CodigoReserva));

            var total = query.Count();

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .ToList();

            var data = items.Select(ReservaDataMapper.ToDataModel);

            return new DataPagedResult<ReservaDataModel>(data, total, filtro.Page, filtro.PageSize);
        }

        // =========================
        // DISPONIBILIDAD 🔥
        // =========================

        public async Task<bool> IsVehiculoDisponibleAsync(
            int idVehiculo,
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken ct = default)
        {
            var reservas = await _unitOfWork.Reservas.GetAllAsync(ct);

            return !reservas.Any(r =>
                r.id_vehiculo == idVehiculo &&
                !r.es_eliminado &&
                r.estado_reserva != "CAN" &&
                (
                    fechaInicio < r.fecha_fin &&
                    fechaFin > r.fecha_inicio
                ));
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task<ReservaDataModel> CreateAsync(ReservaDataModel model, CancellationToken ct = default)
        {
            var disponible = await IsVehiculoDisponibleAsync(
                model.IdVehiculo,
                model.FechaInicio,
                model.FechaFin,
                ct);

            if (!disponible)
                throw new Exception("Vehículo no disponible en ese rango");

            var entity = ReservaDataMapper.ToEntity(model);

            entity.guid_reserva = Guid.NewGuid();
            entity.fecha_reserva_utc = DateTime.UtcNow;
            entity.estado_reserva = "PEN";
            entity.es_eliminado = false;

            entity.cantidad_dias = (int)(entity.fecha_fin - entity.fecha_inicio).TotalDays;

            await _unitOfWork.Reservas.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return ReservaDataMapper.ToDataModel(entity);
        }

        public async Task<ReservaDataModel> UpdateAsync(ReservaDataModel model, CancellationToken ct = default)
        {
            var existing = await _unitOfWork.Reservas.GetByIdAsync(model.Id, ct);

            if (existing == null)
                throw new Exception("Reserva no encontrada");

            existing.fecha_inicio = model.FechaInicio;
            existing.fecha_fin = model.FechaFin;

            existing.descripcion_reserva = model.Descripcion;
            existing.origen_canal_reserva = model.OrigenCanal;

            existing.fecha_modificacion_utc = DateTime.UtcNow;
            existing.modificado_por_usuario = model.ModificadoPorUsuario;

            await _unitOfWork.Reservas.UpdateAsync(existing, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return ReservaDataMapper.ToDataModel(existing);
        }

        // =========================
        // ACCIONES DE NEGOCIO
        // =========================

        public async Task<bool> ConfirmarAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Reservas.GetByIdAsync(id, ct);

            if (entity == null || entity.estado_reserva != "PEN")
                return false;

            entity.estado_reserva = "CON";
            entity.fecha_confirmacion_utc = DateTime.UtcNow;

            await _unitOfWork.Reservas.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> CancelarAsync(int id, string motivo, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Reservas.GetByIdAsync(id, ct);

            if (entity == null || entity.estado_reserva == "CAN")
                return false;

            entity.estado_reserva = "CAN";
            entity.fecha_cancelacion_utc = DateTime.UtcNow;
            entity.motivo_cancelacion = motivo;

            await _unitOfWork.Reservas.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }
    }
}