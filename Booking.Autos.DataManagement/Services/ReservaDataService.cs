using Booking.Autos.DataManagement.Common;
using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Mappers;
using Booking.Autos.DataManagement.Models.Reservas;

namespace Booking.Autos.DataManagement.Services
{
    public class ReservaDataService : IReservaDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservaDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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
            return entities.Where(x => x.id_cliente == idCliente && !x.es_eliminado).Select(ReservaDataMapper.ToDataModel);
        }

        public async Task<IEnumerable<ReservaDataModel>> GetByVehiculoAsync(int idVehiculo, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Reservas.GetAllAsync(ct);
            return entities.Where(x => x.id_vehiculo == idVehiculo && !x.es_eliminado).Select(ReservaDataMapper.ToDataModel);
        }

        public async Task<DataPagedResult<ReservaDataModel>> BuscarAsync(ReservaFiltroDataModel filtro, CancellationToken ct = default)
        {
            var query = (await _unitOfWork.Reservas.GetAllAsync(ct)).AsQueryable();

            if (filtro.IdCliente.HasValue)
                query = query.Where(x => x.id_cliente == filtro.IdCliente);

            if (filtro.IdVehiculo.HasValue)
                query = query.Where(x => x.id_vehiculo == filtro.IdVehiculo);

            if (filtro.IdLocalizacionRecogida.HasValue)
                query = query.Where(x => x.id_localizacion_recogida == filtro.IdLocalizacionRecogida);

            if (filtro.IdLocalizacionEntrega.HasValue)
                query = query.Where(x => x.id_localizacion_entrega == filtro.IdLocalizacionEntrega);

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(x => x.estado_reserva == filtro.Estado);

            if (filtro.FechaInicioDesde.HasValue)
                query = query.Where(x => x.fecha_inicio >= filtro.FechaInicioDesde);

            if (filtro.FechaInicioHasta.HasValue)
                query = query.Where(x => x.fecha_inicio <= filtro.FechaInicioHasta);

            if (filtro.FechaFinDesde.HasValue)
                query = query.Where(x => x.fecha_fin >= filtro.FechaFinDesde);

            if (filtro.FechaFinHasta.HasValue)
                query = query.Where(x => x.fecha_fin <= filtro.FechaFinHasta);

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
                (fechaInicio < r.fecha_fin && fechaFin > r.fecha_inicio));
        }

        public async Task<ReservaDataModel> CreateAsync(ReservaDataModel model, CancellationToken ct = default)
        {
            var disponible = await IsVehiculoDisponibleAsync(model.IdVehiculo, model.FechaInicio, model.FechaFin, ct);
            if (!disponible)
                throw new InvalidOperationException("El vehículo no está disponible para el rango de fechas solicitado.");

            var entity = ReservaDataMapper.ToEntity(model);
            entity.cantidad_dias = (int)(entity.fecha_fin - entity.fecha_inicio).TotalDays;

            await _unitOfWork.Reservas.AddAsync(entity, ct);

            return ReservaDataMapper.ToDataModel(entity);
        }

        public async Task<ReservaDataModel> UpdateAsync(ReservaDataModel model, CancellationToken ct = default)
        {
            var existing = await _unitOfWork.Reservas.GetByIdAsync(model.Id, ct);

            if (existing == null)
                throw new Exception("Reserva no encontrada");

            existing.fecha_inicio = model.FechaInicio;
            existing.fecha_fin = model.FechaFin;
            existing.hora_inicio = model.HoraInicio;
            existing.hora_fin = model.HoraFin;
            existing.id_localizacion_recogida = model.IdLocalizacionRecogida;
            existing.id_localizacion_entrega = model.IdLocalizacionEntrega;
            existing.cantidad_dias = model.CantidadDias;
            existing.subtotal_reserva = model.Subtotal;
            existing.valor_iva = model.Iva;
            existing.total_reserva = model.Total;
            existing.descripcion_reserva = model.Descripcion;
            existing.origen_canal_reserva = model.OrigenCanal;
            existing.fecha_modificacion_utc = DateTime.UtcNow;
            existing.modificado_por_usuario = model.ModificadoPorUsuario;
            existing.modificacion_ip = model.ModificacionIp;

            await _unitOfWork.Reservas.UpdateAsync(existing, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return ReservaDataMapper.ToDataModel(existing);
        }

        public async Task<bool> ConfirmarAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Reservas.GetByIdAsync(id, ct);

            if (entity == null || entity.estado_reserva != "PEN")
                return false;

            entity.estado_reserva = "CON";
            entity.fecha_confirmacion_utc = DateTime.UtcNow;
            entity.fecha_modificacion_utc = DateTime.UtcNow;

            await _unitOfWork.Reservas.UpdateAsync(entity, ct);

            var extras = await _unitOfWork.ReservasExtras.GetByReservaAsync(id, ct);
            foreach (var extra in extras)
            {
                extra.r_x_e_estado = "CON";
                extra.fecha_actualizacion = DateTime.UtcNow;
                await _unitOfWork.ReservasExtras.UpdateAsync(extra, ct);
            }

            var conductores = await _unitOfWork.ConductoresReservas.GetByReservaIdAsync(id, ct);
            foreach (var conductor in conductores)
            {
                conductor.estado_asignacion = "CON";
                conductor.fecha_modificacion_utc = DateTime.UtcNow;
                await _unitOfWork.ConductoresReservas.UpdateAsync(conductor, ct);
            }

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
            entity.fecha_modificacion_utc = DateTime.UtcNow;

            await _unitOfWork.Reservas.UpdateAsync(entity, ct);

            var extras = await _unitOfWork.ReservasExtras.GetByReservaAsync(id, ct);
            foreach (var extra in extras)
            {
                extra.r_x_e_estado = "CAN";
                extra.fecha_actualizacion = DateTime.UtcNow;
                await _unitOfWork.ReservasExtras.UpdateAsync(extra, ct);
            }

            var conductores = await _unitOfWork.ConductoresReservas.GetByReservaIdAsync(id, ct);
            foreach (var conductor in conductores)
            {
                conductor.estado_asignacion = "CAN";
                conductor.fecha_desasignacion_utc = DateTime.UtcNow;
                conductor.fecha_modificacion_utc = DateTime.UtcNow;
                await _unitOfWork.ConductoresReservas.UpdateAsync(conductor, ct);
            }

            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }
    }
}
