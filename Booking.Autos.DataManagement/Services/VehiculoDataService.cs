using Booking.Autos.DataManagement.Interfaces;
using Booking.Autos.DataManagement.Models.Vehiculos;
using Booking.Autos.DataManagement.Mappers;
using Booking.Autos.DataManagement.Common;

namespace Booking.Autos.DataManagement.Services
{
    public class VehiculoDataService : IVehiculoDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehiculoDataService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================
        // CONSULTAS
        // =========================

        public async Task<IReadOnlyList<VehiculoDataModel>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Vehiculos.GetAllAsync(ct);

            return VehiculoDataMapper.ToDataModelList(entities);
        }

        public async Task<VehiculoDataModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.VehiculoDetalleQueries
                .GetDetalleAsync(id, ct);

            return entity == null
                ? null
                : VehiculoDataMapper.ToDataModel(entity);
        }

        public async Task<VehiculoDataModel?> GetByPlacaAsync(string placa, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Vehiculos.GetAllAsync(ct);

            var entity = entities.FirstOrDefault(x =>
                x.placa_vehiculo == placa &&
                !x.es_eliminado);

            return entity == null
                ? null
                : VehiculoDataMapper.ToDataModel(entity);
        }

        // =========================
        // 🔥 BUSCADOR PRINCIPAL
        // =========================

        public async Task<DataPagedResult<VehiculoDataModel>> BuscarAsync(
            VehiculoFiltroDataModel filtro,
            CancellationToken ct = default)
        {
            if (!filtro.FechaInicio.HasValue || !filtro.FechaFin.HasValue)
                throw new Exception("Debe enviar fechas para buscar disponibilidad");

            if (!filtro.IdLocalizacion.HasValue)
                throw new Exception("Debe enviar la localización");

            var entities = await _unitOfWork.VehiculoBusquedaQueries.ExecuteAsync(
                filtro.IdLocalizacion.Value,
                filtro.FechaInicio.Value,
                filtro.FechaFin.Value,
                filtro.IdCategoria,
                filtro.IdMarca,
                filtro.TipoTransmision,
                ct
            );

            var query = entities.AsQueryable();

            // 🔥 filtros adicionales (en memoria)
            if (filtro.PrecioMin.HasValue)
                query = query.Where(x => x.precio_base_dia >= filtro.PrecioMin);

            if (filtro.PrecioMax.HasValue)
                query = query.Where(x => x.precio_base_dia <= filtro.PrecioMax);

            if (!string.IsNullOrWhiteSpace(filtro.Modelo))
                query = query.Where(x => x.modelo_vehiculo.Contains(filtro.Modelo));

            if (filtro.CapacidadMinPasajeros.HasValue)
                query = query.Where(x => x.capacidad_pasajeros >= filtro.CapacidadMinPasajeros);

            if (filtro.AireAcondicionado.HasValue)
                query = query.Where(x => x.aire_acondicionado == filtro.AireAcondicionado);

            var total = query.Count();

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .ToList();

            var data = items.Select(VehiculoDataMapper.ToDataModel);

            return new DataPagedResult<VehiculoDataModel>(
                data,
                total,
                filtro.Page,
                filtro.PageSize
            );
        }

        // =========================
        // FILTROS SIMPLES
        // =========================

        public async Task<IEnumerable<VehiculoDataModel>> GetByMarcaAsync(int idMarca, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Vehiculos.GetAllAsync(ct);

            return entities
                .Where(x => x.id_marca == idMarca && !x.es_eliminado)
                .Select(VehiculoDataMapper.ToDataModel);
        }

        public async Task<IEnumerable<VehiculoDataModel>> GetByCategoriaAsync(int idCategoria, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Vehiculos.GetAllAsync(ct);

            return entities
                .Where(x => x.id_categoria == idCategoria && !x.es_eliminado)
                .Select(VehiculoDataMapper.ToDataModel);
        }

        public async Task<IEnumerable<VehiculoDataModel>> GetDisponiblesAsync(CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Vehiculos.GetAllAsync(ct);

            return entities
                .Where(x => x.estado_vehiculo == "DIS" && !x.es_eliminado)
                .Select(VehiculoDataMapper.ToDataModel);
        }

        public async Task<IEnumerable<VehiculoDataModel>> GetByRangoPrecioAsync(decimal min, decimal max, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Vehiculos.GetAllAsync(ct);

            return entities
                .Where(x => x.precio_base_dia >= min && x.precio_base_dia <= max)
                .Select(VehiculoDataMapper.ToDataModel);
        }

        // =========================
        // DISPONIBILIDAD 🔥
        // =========================

        public async Task<bool> IsDisponibleAsync(
            int idVehiculo,
            DateTime fechaInicio,
            DateTime fechaFin,
            CancellationToken ct = default)
        {
            // ⚠️ necesitas localización → puedes obtenerla del vehículo
            var vehiculo = await _unitOfWork.Vehiculos.GetByIdAsync(idVehiculo, ct);

            if (vehiculo == null)
                return false;

            return await _unitOfWork.VehiculoDisponibilidadQueries
                .IsDisponibleAsync(
                    idVehiculo,
                    fechaInicio,
                    fechaFin,
                    vehiculo.localizacion_actual,
                    ct
                );
        }

        // =========================
        // ESCRITURA
        // =========================

        public async Task<VehiculoDataModel> CreateAsync(VehiculoDataModel model, CancellationToken ct = default)
        {
            var entity = VehiculoDataMapper.ToEntity(model);

            entity.vehiculo_guid = Guid.NewGuid();
            entity.fecha_registro_utc = DateTime.UtcNow;
            entity.es_eliminado = false;

            await _unitOfWork.Vehiculos.AddAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return VehiculoDataMapper.ToDataModel(entity);
        }

        public async Task<VehiculoDataModel> UpdateAsync(VehiculoDataModel model, CancellationToken ct = default)
        {
            var existing = await _unitOfWork.Vehiculos.GetByIdAsync(model.Id, ct);

            if (existing == null)
                throw new Exception("Vehículo no encontrado");

            existing.modelo_vehiculo = model.Modelo;
            existing.precio_base_dia = model.PrecioBaseDia;
            existing.color_vehiculo = model.Color;

            existing.fecha_modificacion_utc = DateTime.UtcNow;

            await _unitOfWork.Vehiculos.UpdateAsync(existing, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return VehiculoDataMapper.ToDataModel(existing);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Vehiculos.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            entity.es_eliminado = true;
            entity.fecha_inhabilitacion_utc = DateTime.UtcNow;

            await _unitOfWork.Vehiculos.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        // =========================
        // OPERACIONES
        // =========================

        public async Task<bool> UpdateKilometrajeAsync(int id, int nuevoKilometraje, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Vehiculos.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            entity.kilometraje_actual = nuevoKilometraje;

            await _unitOfWork.Vehiculos.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> UpdateEstadoAsync(int id, string estado, CancellationToken ct = default)
        {
            var entity = await _unitOfWork.Vehiculos.GetByIdAsync(id, ct);

            if (entity == null)
                return false;

            entity.estado_vehiculo = estado;

            await _unitOfWork.Vehiculos.UpdateAsync(entity, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return true;
        }

        // =========================
        // VALIDACIONES
        // =========================

        public async Task<bool> ExistsByPlacaAsync(string placa, CancellationToken ct = default)
        {
            var entities = await _unitOfWork.Vehiculos.GetAllAsync(ct);

            return entities.Any(x =>
                x.placa_vehiculo == placa &&
                !x.es_eliminado);
        }
    }
}